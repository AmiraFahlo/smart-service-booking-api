using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Application.Features.Reviews.Extensions;
using SmartServiceBooking.Domain.Shared;
using SmartServiceBooking.Application.Common.Extensions;

namespace SmartServiceBooking.Application.Features.Reviews.Queries.GetProviderReviews;

public class GetProviderReviewsQueryHandler : IRequestHandler<GetProviderReviewsQuery, Result<PagedList<ReviewDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetProviderReviewsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PagedList<ReviewDto>>> Handle(GetProviderReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviewsQuery = _context.Reviews
            .AsNoTracking()
            .Where(r => r.ServiceProviderId == request.ProviderId)
            .OrderByDescending(r => r.CreatedAt)
            .MapToDto();

        var pagedReviews = await reviewsQuery.ToPagedListAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return Result.Success(pagedReviews);
    }
}
