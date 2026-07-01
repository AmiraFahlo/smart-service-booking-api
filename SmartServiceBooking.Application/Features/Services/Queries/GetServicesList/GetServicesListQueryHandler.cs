namespace SmartServiceBooking.Application.Features.Services.Queries.GetServicesList;

public class GetServicesListQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetServicesListQuery, Result<PagedList<ServiceLookupDto>>>
{
    public async Task<Result<PagedList<ServiceLookupDto>>> Handle(
        GetServicesListQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Services
            .AsNoTracking()
            .Where(s => s.IsActive)
            .ApplySearch(request.SearchTerm)
            .ApplyFilters(request.CategoryId, request.MinPrice, request.MaxPrice)
            .ApplySorting(request.SortBy);

        return await query
            .MapToLookupDto()
            .ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
