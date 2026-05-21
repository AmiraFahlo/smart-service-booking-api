namespace SmartServiceBooking.Application.Features.Services.Queries.GetServicesList;

public class GetServicesListQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetServicesListQuery, Result<PagedList<ServiceLookupDto>>>
{
    public async Task<Result<PagedList<ServiceLookupDto>>> Handle(
        GetServicesListQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Services
            .Include(s => s.Category)
            .AsNoTracking()
            .Where(s => s.IsActive);

        // 1. Text Search (SARGable Search using EF.Functions.Like)
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var likeTerm = $"%{request.SearchTerm.Trim()}%";
            query = query.Where(s => EF.Functions.Like(s.Name, likeTerm) ||
                                     (s.Description != null && EF.Functions.Like(s.Description, likeTerm)) ||
                                     EF.Functions.Like(s.Category.Name, likeTerm));
        }

        // 2. Filters
        if (request.CategoryId.HasValue)
        {
            query = query.Where(s => s.CategoryId == request.CategoryId.Value);
        }

        if (request.MinPrice.HasValue)
        {
            query = query.Where(s => s.BasePrice >= request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(s => s.BasePrice <= request.MaxPrice.Value);
        }

        // 3. Sorting
        query = request.SortBy?.ToLower() switch
        {
            "price_asc" => query.OrderBy(s => s.BasePrice),
            "price_desc" => query.OrderByDescending(s => s.BasePrice),
            "name_asc" => query.OrderBy(s => s.Name),
            _ => query.OrderByDescending(s => s.CreatedAt)
        };

        // 4. Optimized Mapping & Pagination
        return await query
            .MapToLookupDto()
            .ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
