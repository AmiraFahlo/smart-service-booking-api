using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.Identity;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Identity;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users", "Identity");
        
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd(); // Or standard Guid

        builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.UserName).HasMaxLength(256);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
        builder.Property(u => u.Email).HasMaxLength(256);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(256);
        builder.Property(u => u.PhoneNumber).HasMaxLength(50);
        
        builder.HasIndex(u => u.NormalizedUserName).IsUnique().HasFilter("[NormalizedUserName] IS NOT NULL");
        builder.HasIndex(u => u.NormalizedEmail).IsUnique().HasFilter("[NormalizedEmail] IS NOT NULL");

        // Relationships
        //builder.HasMany(u => u.Addresses)
        //    .WithOne(a => a.User)
        //    .HasForeignKey(a => a.UserId)
        //    .OnDelete(DeleteBehavior.Restrict);

        //builder.HasMany(u => u.Wallets)
        //    .WithOne(w => w.User)
        //    .HasForeignKey(w => w.UserId)
        //    .OnDelete(DeleteBehavior.Restrict);
    }
}
