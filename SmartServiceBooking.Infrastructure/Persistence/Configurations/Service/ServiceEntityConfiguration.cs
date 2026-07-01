namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Service;

public class ServiceEntityConfiguration : IEntityTypeConfiguration<ServiceEntity>
{
    public void Configure(EntityTypeBuilder<ServiceEntity> builder)
    {
        builder.ToTable("Services", "Service");
        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.BasePrice).HasColumnType("decimal(18,2)");
        builder.Property(s => s.PriceUnit).HasConversion<string>();

        builder.HasOne(s => s.Category)
            .WithMany(c => c.Services)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => s.CategoryId);
        builder.HasIndex(s => s.BasePrice);
        builder.HasIndex(s => s.Name);

        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}
