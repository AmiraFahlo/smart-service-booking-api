namespace SmartServiceBookingPlatform_API.Extensions;

/// <summary>
/// Centralized authorization policy definitions.
/// All named policies consumed across the application (controllers, minimal API
/// endpoints, OpenAPI spec protection, etc.) are registered here — in one place.
///
/// Adding a new policy:
///   1. Add a constant name to <see cref="PolicyNames"/>.
///   2. Define the policy in <see cref="AddAuthorizationPolicies"/>.
///   3. Consume it via [Authorize("PolicyName")] or .RequireAuthorization("PolicyName").
/// </summary>
public static class AuthorizationExtensions
{
    /// <summary>
    /// Well-known policy name constants.
    /// Using constants avoids magic strings scattered across controllers and extensions.
    /// </summary>
    public static class PolicyNames
    {
        /// <summary>
        /// Requires an authenticated user with the "Admin" role.
        /// Used by: admin-only controllers, production OpenAPI spec endpoint.
        /// </summary>
        public const string AdminOnly = nameof(AdminOnly);
    }

    /// <summary>
    /// Registers all application authorization policies.
    /// Call once from Program.cs / DI setup — before any middleware that references them.
    /// </summary>
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
                .AddPolicy(PolicyNames.AdminOnly, policy =>
                    policy.RequireAuthenticatedUser()
                          .RequireRole("Admin"));

        return services;
    }
}
