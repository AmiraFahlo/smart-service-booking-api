using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Domain.Errors;

public static class ReviewErrors
{
    public static readonly Error InvalidRating = new(
        "Review.InvalidRating",
        "Rating must be between 1 and 5 stars.",
        ErrorType.Validation);

    public static readonly Error Duplicate = new(
        "Review.Duplicate",
        "You have already reviewed this booking.",
        ErrorType.Conflict);

    public static readonly Error BookingNotCompleted = new(
        "Review.BookingNotCompleted",
        "The booking cannot be reviewed until it is completed.",
        ErrorType.Validation);

    public static readonly Error Forbidden = new(
        "Review.Forbidden",
        "You do not have permission to review this booking.",
        ErrorType.Forbidden);
}
