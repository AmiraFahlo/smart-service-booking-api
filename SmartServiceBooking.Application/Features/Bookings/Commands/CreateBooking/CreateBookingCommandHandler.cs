using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Shared;
using SmartServiceBooking.Domain.Entities.Booking;
using SmartServiceBooking.Domain.Entities.Service;
using SmartServiceBooking.Domain.Errors;
using SmartServiceBooking.Domain.Enums;

namespace SmartServiceBooking.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<long>>
{
    private readonly IApplicationDbContext _context;
    private readonly IBookingRepository _bookingRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateBookingCommandHandler(
        IApplicationDbContext context,
        IBookingRepository bookingRepository,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _bookingRepository = bookingRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<long>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch Customer ID
        var customerId = _currentUserService.ParsedUserId;
        if (customerId == null)
        {
            return Result.Failure<long>(new Error("Auth.Unauthorized", "User is not authenticated.", ErrorType.Unauthorized));
        }

        // 2. Fetch Provider Service and price details
        var providerService = await _context.ProviderServices
            .Include(ps => ps.Service)
            .FirstOrDefaultAsync(ps => 
                ps.ProviderId == request.ProviderId && 
                ps.ServiceId == request.ServiceId && 
                ps.IsActive, 
                cancellationToken);

        if (providerService == null || providerService.Service == null)
        {
            return Result.Failure<long>(BookingErrors.ServiceNotFound);
        }

        var service = providerService.Service;
        var price = providerService.CustomPrice ?? service.BasePrice;

        // 3. Verify Provider Availability
        var dayOfWeek = request.ScheduledStart.DayOfWeek;
        var availability = await _context.ProviderAvailabilities
            .FirstOrDefaultAsync(pa => 
                pa.ProviderId == request.ProviderId && 
                pa.DayOfWeek == dayOfWeek, 
                cancellationToken);

        if (availability == null)
        {
            return Result.Failure<long>(BookingErrors.ProviderUnavailable);
        }

        var bookingStart = TimeOnly.FromDateTime(request.ScheduledStart);
        var bookingEnd = TimeOnly.FromDateTime(request.ScheduledEnd);

        if (bookingStart < availability.StartTime || bookingEnd > availability.EndTime)
        {
            return Result.Failure<long>(BookingErrors.ProviderUnavailable);
        }

        // 4. Verify No Overlapping Bookings
        var date = request.ScheduledStart.Date;
        var hasOverlap = await _bookingRepository.HasOverlappingBookingAsync(
            request.ProviderId,
            date,
            bookingStart,
            bookingEnd,
            cancellationToken);

        if (hasOverlap)
        {
            return Result.Failure<long>(BookingErrors.SlotOverlapped);
        }

        // 5. Calculate Total Amount
        decimal totalAmount;
        if (service.PriceUnit == PriceUnit.Hourly)
        {
            var duration = request.ScheduledEnd - request.ScheduledStart;
            var durationHours = (decimal)duration.TotalHours;
            if (durationHours <= 0)
            {
                return Result.Failure<long>(BookingErrors.InvalidTimeRange);
            }
            totalAmount = price * durationHours;
        }
        else
        {
            totalAmount = price;
        }

        // 6. Instantiate Domain Booking and Save
        var bookingResult = Booking.Create(
            customerId.Value,
            request.ProviderId,
            request.ServiceId,
            request.ScheduledStart,
            request.ScheduledEnd,
            totalAmount,
            request.Notes);

        if (bookingResult.IsFailure)
        {
            return Result.Failure<long>(bookingResult.Error);
        }

        var booking = bookingResult.Value;

        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(booking.Id);
    }
}
