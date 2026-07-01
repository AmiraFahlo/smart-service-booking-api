using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Application.Common.Extensions;
using SmartServiceBooking.Domain.Shared;

using SmartServiceBooking.Application.Features.Bookings.Extensions;

namespace SmartServiceBooking.Application.Features.Bookings.Queries.GetMyBookings;

public class GetMyBookingsQueryHandler : IRequestHandler<GetMyBookingsQuery, Result<PagedList<BookingDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyBookingsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PagedList<BookingDto>>> Handle(GetMyBookingsQuery request, CancellationToken cancellationToken)
    {
        var customerId = _currentUserService.ParsedUserId;
        if (customerId == null)
            return Result.Failure<PagedList<BookingDto>>(new Error("Auth.Unauthorized", "User is not authenticated.", ErrorType.Unauthorized));

        var query = _context.Bookings
            .AsNoTracking()
            .Where(b => b.CustomerId == customerId.Value)
            .Where(b => request.Status == null || b.BookingStatus.ToString() == request.Status)
            .OrderByDescending(b => b.CreatedAt)
            .MapToDto();

        var pagedResult = await query.ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);
        return Result.Success(pagedResult);
    }
}
