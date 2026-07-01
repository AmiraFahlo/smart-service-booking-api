using SmartServiceBooking.Application.Features.Bookings.Queries;
using SmartServiceBooking.Domain.Entities.Booking;

namespace SmartServiceBooking.Application.Features.Bookings.Extensions;

public static class BookingQueryExtensions
{
    public static IQueryable<BookingDto> MapToDto(this IQueryable<Booking> query)
    {
        return query.Select(b => new BookingDto
        {
            Id = b.Id,
            BookingStatus = b.BookingStatus.ToString(),
            PaymentStatus = b.PaymentStatus.ToString(),
            ScheduledStart = b.ScheduledStart,
            ScheduledEnd = b.ScheduledEnd,
            TotalAmount = b.TotalAmount,
            Notes = b.Notes,
            CreatedAt = b.CreatedAt,
            ServiceId = b.ServiceId,
            ServiceName = b.Service != null ? b.Service.Name : string.Empty,
            CustomerId = b.CustomerId,
            CustomerName = b.Customer != null
                ? b.Customer.FirstName + " " + b.Customer.LastName
                : string.Empty,
            ProviderId = b.ProviderId,
            ProviderName = b.Provider != null && b.Provider.User != null
                ? b.Provider.User.FirstName + " " + b.Provider.User.LastName
                : string.Empty
        });
    }
}
