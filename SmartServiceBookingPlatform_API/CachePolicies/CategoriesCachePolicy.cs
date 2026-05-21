using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace SmartServiceBookingPlatform_API.CachePolicies;

/// <summary>
/// Output Cache Policy for Categories endpoints.
/// Duration: 10 minutes | VaryByQuery: all params
/// Invalidation Tag: "categories-tag"
/// Categories change rarely so 10-min TTL is appropriate.
/// </summary>
public sealed class CategoriesCachePolicy : IOutputCachePolicy
{
    public static readonly string Tag = "categories-tag";

    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        var isGetRequest = HttpMethods.IsGet(context.HttpContext.Request.Method);
        context.EnableOutputCaching = isGetRequest;
        context.AllowCacheLookup = isGetRequest;
        context.AllowCacheStorage = isGetRequest;
        context.AllowLocking = true;

        // Cache duration: 10 minutes (categories change rarely)
        context.ResponseExpirationTimeSpan = TimeSpan.FromMinutes(10);

        // Vary by all query string params
        context.CacheVaryByRules.QueryKeys = StringValues.Empty;

        // Tag for invalidation
        context.Tags.Add(Tag);

        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;

    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        var response = context.HttpContext.Response;

        if (response.StatusCode != StatusCodes.Status200OK)
        {
            context.AllowCacheStorage = false;
        }

        return ValueTask.CompletedTask;
    }
}
