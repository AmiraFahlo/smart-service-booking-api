namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetProvidersByService;

public class GetProvidersByServiceQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetProvidersByServiceQuery, Result<PagedList<ProviderLookupDto>>>
{
    public async Task<Result<PagedList<ProviderLookupDto>>> Handle(
        GetProvidersByServiceQuery request,
        CancellationToken cancellationToken)
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

        var result = await query.ToPagedListAsync(
            request.PageNumber, 
            request.PageSize, 
            cancellationToken);

        return Result.Success(result);
    }
}
