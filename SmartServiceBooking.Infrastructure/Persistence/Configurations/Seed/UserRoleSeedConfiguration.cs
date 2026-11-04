using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.Identity;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed;

public class UserRoleSeedConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.HasData(
            new ApplicationUserRole { UserId = 1, RoleId = 1 }, // Admin
            new ApplicationUserRole { UserId = 1, RoleId = 3 }, // ServiceProvider (Solar - Furqan)
            new ApplicationUserRole { UserId = 2, RoleId = 3 }, // ServiceProvider (Electric - Hamdaniyah)
            new ApplicationUserRole { UserId = 3, RoleId = 3 }, // ServiceProvider (Mechanic - Sulaymaniyah)
            new ApplicationUserRole { UserId = 4, RoleId = 3 }, // ServiceProvider (AC - Shahbaa)
            new ApplicationUserRole { UserId = 5, RoleId = 3 }  // ServiceProvider (Plumber - Mogambo)
        );

    }
}
