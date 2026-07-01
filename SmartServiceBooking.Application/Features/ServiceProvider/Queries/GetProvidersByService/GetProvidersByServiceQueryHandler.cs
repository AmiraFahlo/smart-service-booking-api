namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetProvidersByService;

public class GetProvidersByServiceQueryHandler(
    IApplicationDbContext context,
    ICacheService cacheService)
    : IRequestHandler<GetProvidersByServiceQuery, Result<PagedList<ProviderLookupDto>>>
{
    public async Task<Result<PagedList<ProviderLookupDto>>> Handle(
        GetProvidersByServiceQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"providers-by-service-{request.ServiceId}-p{request.PageNumber}-s{request.PageSize}";

        var result = await cacheService.GetOrCreateAsync(
            cacheKey,
            async () =>
            {
                // 🚀 Starting from ProviderServices ensures we only see providers mapped to this service
                var query = context.ProviderServices
                    .AsNoTracking()
                    .Where(ps => ps.ServiceId == request.ServiceId && 
                                 ps.IsActive && 
                                 ps.Provider != null && 
                                 !ps.Provider.IsDeleted)
                    .OrderByDescending(ps => ps.Provider!.IsVerified) // Top quality first
                    .ThenByDescending(ps => ps.Provider!.Rating)
                    .MapToLookupDto();

                return await query.ToPagedListAsync(
                    request.PageNumber, 
                    request.PageSize, 
                    cancellationToken);
            },
            TimeSpan.FromMinutes(10), // Cache for 10 minutes to reduce DB load
            cancellationToken);

        return Result.Success(result);
    }
}
