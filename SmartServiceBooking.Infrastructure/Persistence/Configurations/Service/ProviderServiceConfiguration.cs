namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Service;

public class ProviderServiceConfiguration : IEntityTypeConfiguration<ProviderService>
{
    public void Configure(EntityTypeBuilder<ProviderService> builder)
    {
        builder.ToTable("ProviderServices", "Service");
        builder.Property(ps => ps.CustomPrice).HasColumnType("decimal(18,2)");
        builder.Property(ps => ps.CustomPriceInUSD).HasColumnType("decimal(18,2)");
        
        builder.HasOne(ps => ps.Provider)
            .WithMany(p => p.Services)
            .HasForeignKey(ps => ps.ProviderId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ps => ps.Service)
            .WithMany(s => s.ProviderServices)
            .HasForeignKey(ps => ps.ServiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasIndex(ps => new { ps.ProviderId, ps.ServiceId }).IsUnique();
    }
}
