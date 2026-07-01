namespace SmartServiceBooking.Application.Features.Bookings.Commands.CancelBooking;

public record CancelBookingCommand(long Id) : IRequest<Result>;
