using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Bookings.Queries.GetBookingById;

public record GetBookingByIdQuery(long Id) : IRequest<Result<BookingDto>>;
