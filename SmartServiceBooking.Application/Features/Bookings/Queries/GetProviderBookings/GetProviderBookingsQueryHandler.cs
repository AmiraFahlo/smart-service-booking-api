using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Application.Common.Extensions;
using SmartServiceBooking.Domain.Shared;

using SmartServiceBooking.Application.Features.Bookings.Extensions;

namespace SmartServiceBooking.Application.Features.Bookings.Queries.GetProviderBookings;

public class GetProviderBookingsQueryHandler : IRequestHandler<GetProviderBookingsQuery, Result<PagedList<BookingDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetProviderBookingsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PagedList<BookingDto>>> Handle(GetProviderBookingsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.ParsedUserId;
        if (userId == null)
            return Result.Failure<PagedList<BookingDto>>(new Error("Auth.Unauthorized", "User is not authenticated.", ErrorType.Unauthorized));

        // Resolve the ServiceProvider profile for this user
        var provider = await _context.ServiceProviders
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId.Value, cancellationToken);

        if (provider == null)
            return Result.Failure<PagedList<BookingDto>>(new Error("Provider.NotFound", "No service provider profile found for this user.", ErrorType.NotFound));

        var query = _context.Bookings
            .AsNoTracking()
            .Where(b => b.ProviderId == provider.Id)
            .Where(b => request.Status == null || b.BookingStatus.ToString() == request.Status)
            .OrderByDescending(b => b.ScheduledStart)
            .MapToDto();

        var pagedResult = await query.ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);
        return Result.Success(pagedResult);
    }
}
