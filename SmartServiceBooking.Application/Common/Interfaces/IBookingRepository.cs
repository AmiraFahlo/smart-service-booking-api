using SmartServiceBooking.Domain.Entities.Booking;

namespace SmartServiceBooking.Application.Common.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task AddAsync(Booking booking, CancellationToken cancellationToken);
    Task<bool> HasOverlappingBookingAsync(int providerId, DateTime date, TimeOnly start, TimeOnly end, CancellationToken cancellationToken);
}
