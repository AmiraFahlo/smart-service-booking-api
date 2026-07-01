using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Bookings.Commands.ConfirmBooking;

public record ConfirmBookingCommand(long Id) : IRequest<Result>;
