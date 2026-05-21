using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Entities.Identity;
using SmartServiceBooking.Domain.Shared;
using RefreshTokenEntity = SmartServiceBooking.Domain.Entities.Identity.RefreshToken;

namespace SmartServiceBooking.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider,
    IApplicationDbContext context)
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 1. Find the user who owns this token (Include tokens to perform rotation logic)
        var user = await userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.Token), cancellationToken);

        if (user is null)
        {
            return Result.Failure<AuthResponse>(AuthErrors.InvalidToken);
        }

        var refreshToken = user.RefreshTokens.Single(t => t.Token == request.Token);

        // 2. TOKEN REUSE DETECTION (Senior Security Pattern)
        // If the token is already revoked, it means it was used before.
        // This is a sign of a compromised token. We must revoke ALL active tokens for this user.
        if (refreshToken.IsRevoked)
        {
            foreach (var activeToken in user.RefreshTokens.Where(t => t.IsActive))
            {
                activeToken.Revoke(replacedByToken: "Reuse Attempt Detected");
            }
            await userManager.UpdateAsync(user);
            return Result.Failure<AuthResponse>(AuthErrors.CompromisedToken);
        }

        // 3. Check if token is expired
        if (refreshToken.IsExpired)
        {
            return Result.Failure<AuthResponse>(AuthErrors.ExpiredToken);
        }

        // 4. TOKEN ROTATION (Issue new Access + Refresh Token)
        var roles = await userManager.GetRolesAsync(user);
        var authResult = jwtProvider.Generate(user, roles);

        // Revoke the old token and link it to the new one
        refreshToken.Revoke(replacedByToken: authResult.RefreshToken);

        // Create the new Refresh Token entity
        var newRefreshToken = RefreshTokenEntity.Create(
            authResult.RefreshToken,
            authResult.RefreshTokenExpiration,
            user.Id
        );

        user.RefreshTokens.Add(newRefreshToken);
        
        // Persist changes
        await userManager.UpdateAsync(user);

        // 5. Return Response
        return Result.Success(new AuthResponse(
            user.Id,
            user.Email!,
            $"{user.FirstName} {user.LastName}",
            authResult.AccessToken,
            authResult.RefreshToken
        ));
    }
}
