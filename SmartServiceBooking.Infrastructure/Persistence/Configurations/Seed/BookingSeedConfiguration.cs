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
            BookingEntity.Create(
                id: 1,
                customerId: 1,
                providerId: 1,
                serviceId: 1,
                bookingStatus: BookingStatus.Confirmed,
                paymentStatus: PaymentStatus.Paid,
                scheduledStart: new DateTime(2025, 1, 2, 10, 0, 0, DateTimeKind.Utc),
                scheduledEnd: new DateTime(2025, 1, 2, 13, 0, 0, DateTimeKind.Utc),
                totalAmount: 180.00m,
                createdAt: new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ),
            BookingEntity.Create(
                id: 2,
                customerId: 2,
                providerId: 2,
                serviceId: 2,
                bookingStatus: BookingStatus.Pending,
                paymentStatus: PaymentStatus.Unpaid,
                scheduledStart: new DateTime(2025, 1, 3, 15, 0, 0, DateTimeKind.Utc),
                scheduledEnd: new DateTime(2025, 1, 3, 16, 0, 0, DateTimeKind.Utc),
                totalAmount: 35.00m,
                createdAt: new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
            )
        );
    }
}
