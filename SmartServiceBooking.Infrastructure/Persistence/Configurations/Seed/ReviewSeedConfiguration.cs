using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.Interaction;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed;

public class ReviewSeedConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasData(
            Review.Create(
                id: 1,
                bookingId: 1,
                reviewerId: 1,
                rateeId: 1,
                rating: 5,
                comment: "Excellent service! Very thorough cleaning.",
                createdAt: new global::System.DateTime(2025, 1, 3, 10, 0, 0, global::System.DateTimeKind.Utc)
            )
        );
    }
}
