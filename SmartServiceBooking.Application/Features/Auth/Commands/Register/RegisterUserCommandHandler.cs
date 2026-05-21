using Microsoft.AspNetCore.Identity;
using SmartServiceBooking.Domain.Constants;
using RefreshTokenEntity = SmartServiceBooking.Domain.Entities.Identity.RefreshToken;
using SmartServiceBooking.Domain.Entities.Identity;

namespace SmartServiceBooking.Application.Features.Auth.Commands.Register;

public record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<Result<AuthResponse>>;

public class RegisterUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider)
    : IRequestHandler<RegisterUserCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Check if user already exists
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return Result.Failure<AuthResponse>(AuthErrors.UserAlreadyExists(request.Email));
        }

        // 2. Create the User object
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // 3. Save User to Database
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var firstError = result.Errors.First();
            return Result.Failure<AuthResponse>(AuthErrors.RegistrationFailed(firstError.Description));
        }

        // 4. Assign Default Role (Customer)
        await userManager.AddToRoleAsync(user, Roles.Customer);

        // 5. Generate Tokens
        var roles = new[] { Roles.Customer };
        var authResult = jwtProvider.Generate(user, roles);

        // 6. Save Refresh Token to Database
        var refreshToken = RefreshTokenEntity.Create(
            authResult.RefreshToken,
            authResult.RefreshTokenExpiration,
            user.Id
        );

        user.RefreshTokens.Add(refreshToken);
        await userManager.UpdateAsync(user);

        // 7. Return AuthResponse
        return Result.Success(new AuthResponse(
            user.Id,
            user.Email!,
            $"{user.FirstName} {user.LastName}",
            authResult.AccessToken,
            authResult.RefreshToken
        ));
    }
}
