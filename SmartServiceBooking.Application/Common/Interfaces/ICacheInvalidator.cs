namespace SmartServiceBooking.Application.Common.Interfaces;

/// <summary>
/// Abstraction over the cache invalidation mechanism.
/// Keeps the Application layer free from infrastructure/framework dependencies.
/// The concrete implementation lives in the API project and wraps IOutputCacheStore.
/// </summary>
public interface ICacheInvalidator
{
    /// <summary>
    /// Invalidates all cached responses associated with the given tag.
    /// </summary>
    Task InvalidateByTagAsync(string tag, CancellationToken cancellationToken = default);
}
