
namespace SmartServiceBooking.Application.Features.Bookings.Commands.CancelBooking;

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IBookingRepository _bookingRepository;
    private readonly ICurrentUserService _currentUserService;

    public CancelBookingCommandHandler(
        IApplicationDbContext context,
        IBookingRepository bookingRepository,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _bookingRepository = bookingRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
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

        // 3. Authorization Check (Customer, Assigned Provider, or Admin)
        var isCustomer = booking.CustomerId == userId.Value;
        var isAdmin = _currentUserService.IsInRole("Admin");
        var isProvider = false;

        if (!isCustomer && !isAdmin)
        {
            var provider = await _context.ServiceProviders
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId.Value, cancellationToken);

            if (provider != null && booking.ProviderId == provider.Id)
            {
                isProvider = true;
            }
        }

        if (!isCustomer && !isProvider && !isAdmin)
        {
            return Result.Failure(new Error("Auth.Forbidden", "You are not authorized to cancel this booking.", ErrorType.Forbidden));
        }

        // 4. Cancel the booking (State Machine)
        var cancelResult = booking.Cancel();
        if (cancelResult.IsFailure)
        {
            return cancelResult;
        }

        // 5. Save changes
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
