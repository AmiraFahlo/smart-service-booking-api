namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Service;
public class ServiceProviderConfiguration : IEntityTypeConfiguration<DomainServiceProvider>
{
    public void Configure(EntityTypeBuilder<DomainServiceProvider> builder)
    {
        builder.ToTable("ServiceProviders", "Service");
        // Id is shared with Users table
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.BusinessName).HasMaxLength(200);
        builder.Property(p => p.Rating).HasColumnType("decimal(3,2)");
        builder.Property(p => p.CommissionRate).HasColumnType("decimal(5,2)");
        builder.Property(p => p.City).HasMaxLength(100).HasDefaultValue("حلب");
        builder.Property(p => p.District).HasMaxLength(100);
        builder.Property(p => p.AddressDetails).HasMaxLength(300);
        builder.Property(p => p.Latitude).HasColumnType("float");
        builder.Property(p => p.Longitude).HasColumnType("float");
        
        builder.HasIndex(p => p.District);
        builder.HasIndex(p => p.City);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
