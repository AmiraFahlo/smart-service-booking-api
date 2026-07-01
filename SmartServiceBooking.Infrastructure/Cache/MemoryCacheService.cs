using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SmartServiceBooking.Application.Common.Interfaces;

namespace SmartServiceBooking.Infrastructure.Cache;

public class MemoryCacheService(IMemoryCache memoryCache) : ICacheService
{
    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var result = await memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(60);
                return factory();
            });

        return result!;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}
