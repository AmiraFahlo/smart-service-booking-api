using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Entities.Booking;
using SmartServiceBooking.Domain.Enums;

namespace SmartServiceBooking.Infrastructure.Persistence.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task AddAsync(Booking booking, CancellationToken cancellationToken)
    {
        await _context.Bookings.AddAsync(booking, cancellationToken);
    }

    public async Task<bool> HasOverlappingBookingAsync(
        int providerId,
        DateTime date,
        TimeOnly start,
        TimeOnly end,
        CancellationToken cancellationToken)
    {
        var startDateTime = date.Date.Add(start.ToTimeSpan());
        var endDateTime = date.Date.Add(end.ToTimeSpan());

        return await _context.Bookings.AnyAsync(b =>
            b.ProviderId == providerId &&
            b.BookingStatus != BookingStatus.Cancelled &&
            b.ScheduledStart < endDateTime &&
            b.ScheduledEnd > startDateTime,
            cancellationToken);
    }
}
