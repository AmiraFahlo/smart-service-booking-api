using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Errors;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Bookings.Commands.ConfirmBooking;

public class ConfirmBookingCommandHandler : IRequestHandler<ConfirmBookingCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IBookingRepository _bookingRepository;
    private readonly ICurrentUserService _currentUserService;

    public ConfirmBookingCommandHandler(
        IApplicationDbContext context,
        IBookingRepository bookingRepository,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _bookingRepository = bookingRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        // 1. Authenticate user
        var userId = _currentUserService.ParsedUserId;
        if (userId == null)
        {
            return Result.Failure(new Error("Auth.Unauthorized", "User is not authenticated.", ErrorType.Unauthorized));
        }

        // 2. Fetch booking
        var booking = await _bookingRepository.GetByIdAsync(request.Id, cancellationToken);
        if (booking == null)
        {
            return Result.Failure(BookingErrors.NotFound(request.Id));
        }

        // 3. Resolve current user's provider profile
        var provider = await _context.ServiceProviders
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId.Value, cancellationToken);

        if (provider == null || booking.ProviderId != provider.Id)
        {
            return Result.Failure(new Error("Auth.Forbidden", "You are not authorized to confirm this booking.", ErrorType.Forbidden));
        }

        // 4. Confirm the booking (State Machine)
        var confirmResult = booking.Confirm();
        if (confirmResult.IsFailure)
        {
            return confirmResult;
        }

        // 5. Save changes
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
