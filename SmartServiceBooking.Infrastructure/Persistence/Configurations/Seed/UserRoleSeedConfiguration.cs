using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.Identity;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed;

public class UserRoleSeedConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.HasData(
     new ApplicationUserRole { UserId = 1, RoleId = 1 },
     new ApplicationUserRole { UserId = 1, RoleId = 2 },
     new ApplicationUserRole { UserId = 2, RoleId = 2 });

    }
}
