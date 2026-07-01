using System.Linq;
using SmartServiceBooking.Application.Features.Reviews.Queries.GetProviderReviews;
using SmartServiceBooking.Domain.Entities.Interaction;

namespace SmartServiceBooking.Application.Features.Reviews.Extensions;

public static class ReviewQueryExtensions
{
    public static IQueryable<ReviewDto> MapToDto(this IQueryable<Review> query)
    {
        return query.Select(r => new ReviewDto(
            r.Customer != null ? r.Customer.FirstName + " " + r.Customer.LastName : "Anonymous",
            r.Rating,
            r.Comment,
            r.CreatedAt
        ));
    }
}
