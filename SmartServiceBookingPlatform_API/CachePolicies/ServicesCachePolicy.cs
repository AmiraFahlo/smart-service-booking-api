using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace SmartServiceBookingPlatform_API.CachePolicies;

/// <summary>
/// Output Cache Policy for Services endpoints.
/// Duration: 5 minutes | VaryByQuery: all params (supports search + pagination)
/// Invalidation Tag: "services-tag"
/// </summary>
public sealed class ServicesCachePolicy : IOutputCachePolicy
{
    public static readonly string Tag = "services-tag";

    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        // Cache only GET requests
        var isGetRequest = HttpMethods.IsGet(context.HttpContext.Request.Method);
        context.EnableOutputCaching = isGetRequest;
        context.AllowCacheLookup = isGetRequest;
        context.AllowCacheStorage = isGetRequest;
        context.AllowLocking = true;

        // Cache duration: 5 minutes
        context.ResponseExpirationTimeSpan = TimeSpan.FromMinutes(5);

        // Vary cache by all query string params (search, page, pageSize, etc.)
        context.CacheVaryByRules.QueryKeys = StringValues.Empty; // Empty = vary by ALL query keys

        // Tag for invalidation
        context.Tags.Add(Tag);

        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;

    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        var response = context.HttpContext.Response;

        // Only cache successful responses
        if (response.StatusCode != StatusCodes.Status200OK)
        {
            context.AllowCacheStorage = false;
        }

        return ValueTask.CompletedTask;
    }
}
