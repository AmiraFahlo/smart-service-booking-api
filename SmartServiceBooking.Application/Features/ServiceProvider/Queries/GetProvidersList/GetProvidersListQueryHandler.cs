using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Shared;
using SmartServiceBooking.Application.Features.ServiceProvider.Extensions;
using SmartServiceBooking.Shared.DTOs.ServiceProvider;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetProvidersList;

public class GetProvidersListQueryHandler(
    IApplicationDbContext context)
    : IRequestHandler<GetProvidersListQuery, Result<PagedList<ServiceProviderProfileDto>>>
{
    public async Task<Result<PagedList<ServiceProviderProfileDto>>> Handle(
        GetProvidersListQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.ServiceProviders
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.Services)
            .Where(p => !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.District))
        {
            query = query.Where(p => p.District == request.District);
        }

        if (request.CategoryId.HasValue)
        {
            query = query.Where(p => p.Services.Any(s => s.Service != null && s.Service.CategoryId == request.CategoryId.Value));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            query = query.Where(p => (p.BusinessName != null && p.BusinessName.Contains(term)) ||
                                     (p.Bio != null && p.Bio.Contains(term)));
        }

        var pagedList = await query
            .OrderByDescending(p => p.IsVerified)
            .ThenByDescending(p => p.Rating)
            .MapToProfileDto()
            .ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);

        return Result.Success(pagedList);
    }
}
