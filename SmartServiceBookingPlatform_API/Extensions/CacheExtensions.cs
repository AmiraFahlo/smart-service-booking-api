using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBookingPlatform_API.CachePolicies;
using SmartServiceBookingPlatform_API.Infrastructure.Cache;

namespace SmartServiceBookingPlatform_API.Extensions;

public static class CacheExtensions
{
    /// <summary>
    /// Registers Output Cache with all domain-specific named policies.
    /// Designed for easy migration to Redis by swapping the store only.
    /// To migrate to Redis in the future:
    ///   1. Install: Microsoft.Extensions.Caching.StackExchangeRedis
    ///   2. Replace: services.AddStackExchangeRedisOutputCache(options => { ... })
    ///   3. No changes needed to Policies, Controllers, or Handlers.
    /// </summary>
    public static IServiceCollection AddOutputCaching(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            // Register named policies so controllers can reference them by constant name
            options.AddPolicy(CachePolicyNames.Services, builder =>
                builder.AddPolicy<ServicesCachePolicy>());

            options.AddPolicy(CachePolicyNames.Categories, builder =>
                builder.AddPolicy<CategoriesCachePolicy>());
        });

        // Register ICacheInvalidator — Application layer depends only on this interface
        services.AddScoped<ICacheInvalidator, OutputCacheInvalidator>();

        return services;
    }

    public static WebApplication UseOutputCaching(this WebApplication app)
    {
        // Must be placed AFTER UseAuthorization and BEFORE MapControllers
        app.UseOutputCache();
        return app;
    }
}

/// <summary>
/// Strongly-typed cache policy name constants — eliminates magic strings in controllers.
/// </summary>
public static class CachePolicyNames
{
    public const string Services = "Services";
    public const string Categories = "Categories";
}
