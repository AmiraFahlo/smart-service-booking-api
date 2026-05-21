using Microsoft.AspNetCore.OutputCaching;
using SmartServiceBooking.Application.Common.Interfaces;

namespace SmartServiceBookingPlatform_API.Infrastructure.Cache;

/// <summary>
/// Concrete implementation of ICacheInvalidator using ASP.NET Core's IOutputCacheStore.
/// Registered in the API layer's DI — Application layer only depends on the interface.
/// To migrate to Redis: swap the IOutputCacheStore store registration in Program.cs only.
/// </summary>
public sealed class OutputCacheInvalidator : ICacheInvalidator
{
    private readonly IOutputCacheStore _store;

    public OutputCacheInvalidator(IOutputCacheStore store)
    {
        _store = store;
    }

    public async Task InvalidateByTagAsync(string tag, CancellationToken cancellationToken = default)
    {
        await _store.EvictByTagAsync(tag, cancellationToken);
    }
}
