using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Bookings.Commands.CreateBooking;

public record CreateBookingCommand(
    int ProviderId,
    long ServiceId,
    DateTime ScheduledStart,
    DateTime ScheduledEnd,
    string? Notes = null) : IRequest<Result<long>>;
