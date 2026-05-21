using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Domain.Constants;
public static class DomainErrors
{
    public static class Auth
    {
        public static readonly Error InvalidCredentials = new(
            "Auth.InvalidCredentials",
            "Invalid email or password.");

        public static readonly Error ValidToken = new(
            "Auth.ValidToken",
            "The token is still valid.");

        public static readonly Error InvalidToken = new(
            "Auth.InvalidToken",
            "The token is invalid.");

        public static readonly Error RoleAssignmentFailed = new(
            "Auth.RoleAssignmentFailed",
            "Failed to assign role to user.");
    }
    public static class User
    {
        public static readonly Error DuplicateEmail = new(
            "User.DuplicateEmail",
            "User with this email already exists.");
            
        public static readonly Error NotFound = new(
            "User.NotFound",
            "User not found.");

        public static readonly Error InvalidRefreshToken = new(
            "User.InvalidRefreshToken",
            "Invalid refresh token.");
    }
}
