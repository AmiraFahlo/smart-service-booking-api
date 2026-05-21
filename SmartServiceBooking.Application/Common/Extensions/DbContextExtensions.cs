using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Common.Extensions;

public static class DbContextExtensions
{
    /// <summary>
    /// Saves changes to the database and invalidates the specified cache tags.
    /// Returns a Success Result if everything succeeds.
    /// </summary>
    public static async Task<Result> SaveAndInvalidateAsync(
        this IApplicationDbContext context,
        ICacheInvalidator cacheInvalidator,
        string cacheTag,
        CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
        
        await cacheInvalidator.InvalidateByTagAsync(cacheTag, cancellationToken);
        
        return Result.Success();
    }

    /// <summary>
    /// Saves changes to the database and invalidates multiple cache tags.
    /// </summary>
    public static async Task<Result> SaveAndInvalidateAsync(
        this IApplicationDbContext context,
        ICacheInvalidator cacheInvalidator,
        IEnumerable<string> cacheTags,
        CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
        
        foreach (var tag in cacheTags)
        {
            await cacheInvalidator.InvalidateByTagAsync(tag, cancellationToken);
        }
        
        return Result.Success();
    }
    
    /// <summary>
    /// Generic version for when a handler returns a value (e.g., Created Id).
    /// </summary>
    public static async Task<Result<T>> SaveAndInvalidateAsync<T>(
        this IApplicationDbContext context,
        T value,
        ICacheInvalidator cacheInvalidator,
        string cacheTag,
        CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
        
        await cacheInvalidator.InvalidateByTagAsync(cacheTag, cancellationToken);
        
        return Result.Success(value);
    }
}
