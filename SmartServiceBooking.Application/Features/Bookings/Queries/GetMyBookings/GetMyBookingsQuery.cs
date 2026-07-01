using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Bookings.Queries.GetMyBookings;

public record GetMyBookingsQuery(
    string? Status = null,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<Result<PagedList<BookingDto>>>;
