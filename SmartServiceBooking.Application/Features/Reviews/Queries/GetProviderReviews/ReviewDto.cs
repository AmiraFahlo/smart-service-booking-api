using System;

namespace SmartServiceBooking.Application.Features.Reviews.Queries.GetProviderReviews;

public record ReviewDto(
    string CustomerName,
    int Rating,
    string? Comment,
    DateTime CreatedAt);
