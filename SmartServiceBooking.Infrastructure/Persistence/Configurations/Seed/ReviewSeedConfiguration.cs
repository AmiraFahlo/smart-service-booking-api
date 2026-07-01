using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.Interaction;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed;

public class ReviewSeedConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasData(
            new
            {
                Id = 1L,
                BookingId = 1L,
                CustomerId = 1,
                ServiceProviderId = 1,
                Rating = 5,
                Comment = "Excellent service! Very thorough cleaning.",
                CreatedAt = new DateTime(2025, 1, 3, 10, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            }
        );
    }
}

