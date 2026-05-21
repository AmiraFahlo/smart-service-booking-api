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
        
        builder.HasIndex(r => new { r.BookingId, r.ReviewerId }).IsUnique();

        builder.HasOne(r => r.Booking)
            .WithMany()
            .HasForeignKey(r => r.BookingId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Reviewer)
            .WithMany()
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(r => r.Ratee)
            .WithMany()
            .HasForeignKey(r => r.RateeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
