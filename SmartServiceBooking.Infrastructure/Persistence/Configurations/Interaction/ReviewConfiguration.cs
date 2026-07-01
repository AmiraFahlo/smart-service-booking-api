using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.Interaction;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Interaction;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews", "Interaction");

        builder.Property(r => r.Comment).HasMaxLength(1000);

        builder.Property(r => r.CustomerId)
            .HasColumnName("ReviewerId");

        builder.Property(r => r.ServiceProviderId)
            .HasColumnName("RateeId");

        builder.HasIndex(r => new { r.BookingId, r.CustomerId }).IsUnique();

        builder.HasOne(r => r.Booking)
            .WithMany()
            .HasForeignKey(r => r.BookingId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Customer)
            .WithMany()
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.ServiceProvider)
            .WithMany()
            .HasForeignKey(r => r.ServiceProviderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

