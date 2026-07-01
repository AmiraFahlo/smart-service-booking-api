using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Bookings.Commands.CompleteBooking;

public record CompleteBookingCommand(long Id) : IRequest<Result>;
