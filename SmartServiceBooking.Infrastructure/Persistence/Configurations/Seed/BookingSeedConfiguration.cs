using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingEntity = SmartServiceBooking.Domain.Entities.Booking.Booking;
using SmartServiceBooking.Domain.Enums;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed;

public class BookingSeedConfiguration : IEntityTypeConfiguration<BookingEntity>
{
    public void Configure(EntityTypeBuilder<BookingEntity> builder)
    {
        builder.HasData(
            new
            {
                Id = 1L,
                CustomerId = 1,
                ProviderId = 1,
                ServiceId = 1L,
                BookingStatus = BookingStatus.Confirmed,
                PaymentStatus = PaymentStatus.Paid,
                ScheduledStart = new DateTime(2025, 1, 2, 10, 0, 0, DateTimeKind.Utc),
                ScheduledEnd = new DateTime(2025, 1, 2, 13, 0, 0, DateTimeKind.Utc),
                TotalAmount = 180.00m,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            },
            new
            {
                Id = 2L,
                CustomerId = 2,
                ProviderId = 2,
                ServiceId = 2L,
                BookingStatus = BookingStatus.Pending,
                PaymentStatus = PaymentStatus.Unpaid,
                ScheduledStart = new DateTime(2025, 1, 3, 15, 0, 0, DateTimeKind.Utc),
                ScheduledEnd = new DateTime(2025, 1, 3, 16, 0, 0, DateTimeKind.Utc),
                TotalAmount = 35.00m,
                CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            }
        );
    }
}
