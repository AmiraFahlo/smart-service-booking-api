using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace SmartServiceBooking.Application.Features.Categories.Queries.GetCategoriesList;

public class GetCategoriesListQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCategoriesListQuery, Result<PagedList<CategoryLookupDto>>>
{
    public async Task<Result<PagedList<CategoryLookupDto>>> Handle(
        GetCategoriesListQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Categories.AsNoTracking();

        if (request.ParentId.HasValue)
            query = query.Where(c => c.ParentId == request.ParentId);

        if (request.IsActive.HasValue)
            query = query.Where(c => c.IsActive == request.IsActive);

        var totalCount = await query.CountAsync(cancellationToken);

        // Single SQL query — ServiceCount computed via sub-select projection (no N+1)
        // Global Query Filter on Services ensures only non-deleted services are counted
        var categories = await query
            .OrderBy(c => c.Level).ThenBy(c => c.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CategoryLookupDto
            {
                Id = c.Id,
                Name = c.Name,
                Level = c.Level,
                ParentId = c.ParentId,
                ParentName = c.Parent != null ? c.Parent.Name : null,
                IsActive = c.IsActive,
                ServiceCount = c.Services.Count()
            })
            .ToListAsync(cancellationToken);

        return PagedList<CategoryLookupDto>.Create(categories, totalCount, request.PageNumber, request.PageSize);
    }
}
