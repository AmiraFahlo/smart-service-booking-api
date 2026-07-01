namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetProviderAvailability;

public class GetProviderAvailabilityQueryHandler(
    IApplicationDbContext context,
    ICacheService cacheService)
    : IRequestHandler<GetProviderAvailabilityQuery, Result<List<ProviderAvailabilityResponseDto>>>
{
    public async Task<Result<List<ProviderAvailabilityResponseDto>>> Handle(GetProviderAvailabilityQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"provider-availability-{request.ProviderId}";

        var availabilities = await cacheService.GetOrCreateAsync(
            cacheKey,
            async () =>
            {
                return await context.ProviderAvailabilities
                    .AsNoTracking()
                    .Where(pa => pa.ProviderId == request.ProviderId)
                    .OrderBy(pa => pa.DayOfWeek)
                    .Select(pa => new ProviderAvailabilityResponseDto(
                        pa.DayOfWeek.ToString(),
                        (int)pa.DayOfWeek,
                        pa.StartTime,
                        pa.EndTime
                    ))
                    .ToListAsync(cancellationToken);
            },
            TimeSpan.FromHours(24), // Cache for 24 hours
            cancellationToken);

        // Even if empty, we return an empty list which is valid.
        return Result.Success(availabilities);
    }
}
