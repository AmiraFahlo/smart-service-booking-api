using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.Identity;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed;
public class UserSeedConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {


        builder.HasData(
            new ApplicationUser
            {
                Id = 1,
                UserName = "provider1",
                NormalizedUserName = "PROVIDER1",
                Email = "p1@test.com",
                NormalizedEmail = "P1@TEST.COM",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User",
                PasswordHash = "AQAAAAIAAYagAAAAEC5jBCCWvc773YHKSswwGb3wMs1ypeUReGCwmsU5vu9u/x5f1fJkRXBZtn98idlyfQ==",
                ConcurrencyStamp = "f90be1ba-2621-453c-9dc1-0521be88ee0a",
                SecurityStamp = "f90be1ba-2621-453c-9dc1-0521be88ee0a"
            },
            new ApplicationUser
            {
                Id = 2,
                UserName = "provider2",
                Email = "p2@test.com",
                FirstName = "Mike",
                LastName = "Plumber",
                PasswordHash = "AQAAAAIAAYagAAAAEANRgqEAvkfgcsO0Zjd60IIA+ispWX8yxdY2lyionPu+gyFDvN1RISG29ZIgmHKewQ==",
                ConcurrencyStamp = "b4aa48fe-1349-4786-9051-7892dfd6d97c",
                SecurityStamp = "b4aa48fe-1349-4786-9051-7892dfd6d97c"
            }
        );
    }
}
