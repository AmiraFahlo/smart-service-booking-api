using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.Service;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Service;

public class ProviderAvailabilityConfiguration : IEntityTypeConfiguration<ProviderAvailability>
{
    public void Configure(EntityTypeBuilder<ProviderAvailability> builder)
    {
        builder.ToTable("ProviderAvailability", "Service");

        builder.HasOne(pa => pa.Provider)
            .WithMany(p => p.Availabilities)
            .HasForeignKey(pa => pa.ProviderId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
