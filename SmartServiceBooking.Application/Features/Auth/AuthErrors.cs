using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Auth;

public static class AuthErrors
{
    public static Error InvalidCredentials =>
        new("Auth.InvalidCredentials", "Invalid login credentials.", ErrorType.Unauthorized);

    public static Error UserAlreadyExists(string email) =>
        new("Auth.UserAlreadyExists", $"User with email '{email}' already exists.", ErrorType.Conflict);

    public static Error RoleAssignmentFailed =>
        new("Auth.RoleAssignmentFailed", "Failed to assign the role to the user.", ErrorType.Failure);

    public static Error RegistrationFailed(string description) =>
        new("Auth.RegistrationFailed", description, ErrorType.Failure);

    public static Error InvalidToken =>
        new("Auth.InvalidToken", "The provided refresh token is invalid.", ErrorType.Unauthorized);

    public static Error ExpiredToken =>
        new("Auth.ExpiredToken", "The refresh token has expired.", ErrorType.Unauthorized);

    public static Error CompromisedToken =>
        new("Auth.CompromisedToken", "Token security breach detected. Please login again.", ErrorType.Unauthorized);
}
