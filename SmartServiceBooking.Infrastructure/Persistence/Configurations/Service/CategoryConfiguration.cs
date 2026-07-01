namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Service;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", "Service");
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.IconUrl).HasMaxLength(500);
        builder.Property(c => c.Path).HasMaxLength(500);

        builder.HasOne(c => c.Parent)
            .WithMany(p => p.SubCategories)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.Name);

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
