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
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
