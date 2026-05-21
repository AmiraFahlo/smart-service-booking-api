using Microsoft.AspNetCore.Identity;
using RefreshTokenEntity = SmartServiceBooking.Domain.Entities.Identity.RefreshToken;
using SmartServiceBooking.Domain.Entities.Identity;

namespace SmartServiceBooking.Application.Features.Auth.Commands.Login;

public record LoginUserCommand(
    string Email,
    string Password) : IRequest<Result<AuthResponse>>;

public class LoginUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider)
    : IRequestHandler<LoginUserCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Find user by email
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Result.Failure<AuthResponse>(AuthErrors.InvalidCredentials);
        }

        // 2. Verify password
        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            return Result.Failure<AuthResponse>(AuthErrors.InvalidCredentials);
        }

        // 3. Get User Roles
        var roles = await userManager.GetRolesAsync(user);

        // 4. Generate Tokens
        var authResult = jwtProvider.Generate(user, roles);

        // 5. Save Refresh Token to Database
        var refreshToken = RefreshTokenEntity.Create(
            authResult.RefreshToken,
            authResult.RefreshTokenExpiration,
            user.Id
        );

        user.RefreshTokens.Add(refreshToken);
        await userManager.UpdateAsync(user);

        // 6. Return AuthResponse
        return Result.Success(new AuthResponse(
            user.Id,
            user.Email!,
            $"{user.FirstName} {user.LastName}",
            authResult.AccessToken,
            authResult.RefreshToken
        ));
    }
}
