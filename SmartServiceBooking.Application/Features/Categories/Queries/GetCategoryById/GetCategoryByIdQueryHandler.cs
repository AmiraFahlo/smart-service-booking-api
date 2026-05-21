using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace SmartServiceBooking.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDetailDto>>
{
    public async Task<Result<CategoryDetailDto>> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        // AsNoTracking: read-only query
        // Global Query Filter excludes deleted categories automatically
        // Single SQL: ServiceCount and SubCategoryCount via sub-select projections (no N+1)
        var dto = await context.Categories
            .AsNoTracking()
            .Where(c => c.Id == request.Id)
            .Select(c => new CategoryDetailDto
            {
                Id = c.Id,
                Name = c.Name,
                Level = c.Level,
                ParentId = c.ParentId,
                ParentName = c.Parent != null ? c.Parent.Name : null,
                IconUrl = c.IconUrl,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                ServiceCount = c.Services.Count(),
                SubCategoryCount = c.SubCategories.Count()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (dto is null)
            return Result.Failure<CategoryDetailDto>(CategoryErrors.NotFound(request.Id));

        return Result.Success(dto);
    }
}
