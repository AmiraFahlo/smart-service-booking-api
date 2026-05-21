using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.Booking;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Booking;

public class BookingConfiguration : IEntityTypeConfiguration<Domain.Entities.Booking.Booking>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Booking.Booking> builder)
    {
        builder.ToTable("Bookings", "Booking");
        
        builder.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(b => b.BookingStatus).HasConversion<string>();
        builder.Property(b => b.PaymentStatus).HasConversion<string>();
        builder.Property(b => b.Notes).HasMaxLength(500);
        
        builder.Property(b => b.RowVersion).IsRowVersion();

        builder.HasOne(b => b.Customer)
            .WithMany() // Assuming no direct collection back for simplicity or check Identity
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Provider)
            .WithMany(p => p.Bookings)
            .HasForeignKey(b => b.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Service)
            .WithMany()
            .HasForeignKey(b => b.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(b => !b.IsDeleted);
    }
}
