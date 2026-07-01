using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Reviews.Queries.GetProviderReviews;

public record GetProviderReviewsQuery(
    int ProviderId,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<Result<PagedList<ReviewDto>>>;
