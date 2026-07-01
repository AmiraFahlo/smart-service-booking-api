using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Entities.Interaction;
using SmartServiceBooking.Domain.Enums;
using SmartServiceBooking.Domain.Errors;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<long>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateReviewCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<long>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch current customer ID
        var customerId = _currentUserService.ParsedUserId;
        if (customerId == null)
        {
            return Result.Failure<long>(new Error(
                "Auth.Unauthorized",
                "User is not authenticated.",
                ErrorType.Unauthorized));
        }

        // 2. Fetch booking
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

        if (booking == null)
        {
            return Result.Failure<long>(BookingErrors.NotFound(request.BookingId));
        }

        // 3. Verify booking belongs to current customer
        if (booking.CustomerId != customerId.Value)
        {
            return Result.Failure<long>(ReviewErrors.Forbidden);
        }

        // 4. Verify booking is completed
        if (booking.BookingStatus != BookingStatus.Completed)
        {
            return Result.Failure<long>(ReviewErrors.BookingNotCompleted);
        }

        // 5. Verify booking hasn't been reviewed already
        var alreadyReviewed = await _context.Reviews
            .AnyAsync(r => r.BookingId == request.BookingId, cancellationToken);

        if (alreadyReviewed)
        {
            return Result.Failure<long>(ReviewErrors.Duplicate);
        }

        // 6. Create Review (Fires Domain Event)
        var reviewResult = Review.Create(
            request.BookingId,
            customerId.Value,
            booking.ProviderId,
            request.Rating,
            request.Comment);

        if (reviewResult.IsFailure)
        {
            return Result.Failure<long>(reviewResult.Error);
        }

        var review = reviewResult.Value;

        // 7. Save to DB
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync(cancellationToken);

        // 8. Return review ID
        return Result.Success(review.Id);
    }
}
