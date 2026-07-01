using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Bookings.Queries.GetProviderBookings;

public record GetProviderBookingsQuery(
    string? Status = null,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<Result<PagedList<BookingDto>>>;
