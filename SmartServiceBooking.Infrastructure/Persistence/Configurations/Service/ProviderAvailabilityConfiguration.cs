namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Service;

public class ProviderAvailabilityConfiguration : IEntityTypeConfiguration<ProviderAvailability>
{
    public void Configure(EntityTypeBuilder<ProviderAvailability> builder)
    {
        builder.ToTable("ProviderAvailabilities", "Service");

        builder.HasKey(pa => pa.Id);

        // 🛡️ Time mapping for TimeOnly (Maps to SQL 'time')
        builder.Property(pa => pa.StartTime)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(pa => pa.EndTime)
            .HasColumnType("time")
            .IsRequired();

        // 🛡️ Constraint: A provider can only have ONE schedule per day
        builder.HasIndex(pa => new { pa.ProviderId, pa.DayOfWeek })
            .IsUnique();

        // 🔗 Relationship
        builder.HasOne(pa => pa.Provider)
            .WithMany(p => p.Availabilities)
            .HasForeignKey(pa => pa.ProviderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
