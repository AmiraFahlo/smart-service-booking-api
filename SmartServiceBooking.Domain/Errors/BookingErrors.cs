using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Domain.Errors;

public static class BookingErrors
{
    public static Error InvalidTransition(BookingStatus current, BookingStatus target) =>
        new("Booking.InvalidTransition", $"Cannot transition booking from state '{current}' to '{target}'.", ErrorType.Conflict);

    public static readonly Error CannotCancelCompleted = new(
        "Booking.CannotCancelCompleted",
        "Cannot cancel a booking that has already been completed.",
        ErrorType.Conflict);

    public static readonly Error AlreadyCancelled = new(
        "Booking.AlreadyCancelled",
        "Booking is already cancelled.",
        ErrorType.Conflict);

    public static readonly Error PastDate = new(
        "Booking.PastDate",
        "Booking scheduled start date cannot be in the past.",
        ErrorType.Validation);

    public static readonly Error InvalidTimeRange = new(
        "Booking.InvalidTimeRange",
        "Booking scheduled end time must be after the start time.",
        ErrorType.Validation);

    public static readonly Error CrossMidnight = new(
        "Booking.CrossMidnight",
        "Booking must start and end on the same calendar day.",
        ErrorType.Validation);

    public static readonly Error ServiceNotFound = new(
        "Booking.ServiceNotFound",
        "The requested service was not found.",
        ErrorType.NotFound);

    public static readonly Error ProviderUnavailable = new(
        "Booking.ProviderUnavailable",
        "The service provider is not available during the requested time slot.",
        ErrorType.Conflict);

    public static readonly Error SlotOverlapped = new(
        "Booking.SlotOverlapped",
        "The requested time slot overlaps with an existing booking.",
        ErrorType.Conflict);

    public static Error NotFound(long id) => new(
        "Booking.NotFound",
        $"Booking with ID '{id}' was not found.",
        ErrorType.NotFound);
}
