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
                UserName = "ammar_solar",
                NormalizedUserName = "AMMAR_SOLAR",
                Email = "ammar@smartbooking.com",
                NormalizedEmail = "AMMAR@SMARTBOOKING.COM",
                EmailConfirmed = true,
                FirstName = "عمار",
                LastName = "الحلبي",
                PhoneNumber = "+963944111222",
                PasswordHash = "AQAAAAIAAYagAAAAEC5jBCCWvc773YHKSswwGb3wMs1ypeUReGCwmsU5vu9u/x5f1fJkRXBZtn98idlyfQ==",
                ConcurrencyStamp = "f90be1ba-2621-453c-9dc1-0521be88ee0a",
                SecurityStamp = "f90be1ba-2621-453c-9dc1-0521be88ee0a"
            },
            new ApplicationUser
            {
                Id = 2,
                UserName = "ahmad_electric",
                NormalizedUserName = "AHMAD_ELECTRIC",
                Email = "ahmad@smartbooking.com",
                NormalizedEmail = "AHMAD@SMARTBOOKING.COM",
                EmailConfirmed = true,
                FirstName = "أحمد",
                LastName = "الشهابي",
                PhoneNumber = "+963955222333",
                PasswordHash = "AQAAAAIAAYagAAAAEANRgqEAvkfgcsO0Zjd60IIA+ispWX8yxdY2lyionPu+gyFDvN1RISG29ZIgmHKewQ==",
                ConcurrencyStamp = "b4aa48fe-1349-4786-9051-7892dfd6d97c",
                SecurityStamp = "b4aa48fe-1349-4786-9051-7892dfd6d97c"
            },
            new ApplicationUser
            {
                Id = 3,
                UserName = "houssam_mechanic",
                NormalizedUserName = "HOUSSAM_MECHANIC",
                Email = "houssam@smartbooking.com",
                NormalizedEmail = "HOUSSAM@SMARTBOOKING.COM",
                EmailConfirmed = true,
                FirstName = "حسام",
                LastName = "الكردي",
                PhoneNumber = "+963933333444",
                PasswordHash = "AQAAAAIAAYagAAAAEANRgqEAvkfgcsO0Zjd60IIA+ispWX8yxdY2lyionPu+gyFDvN1RISG29ZIgmHKewQ==",
                ConcurrencyStamp = "c4aa48fe-1349-4786-9051-7892dfd6d97d",
                SecurityStamp = "c4aa48fe-1349-4786-9051-7892dfd6d97d"
            },
            new ApplicationUser
            {
                Id = 4,
                UserName = "tarek_ac",
                NormalizedUserName = "TAREK_AC",
                Email = "tarek@smartbooking.com",
                NormalizedEmail = "TAREK@SMARTBOOKING.COM",
                EmailConfirmed = true,
                FirstName = "طارق",
                LastName = "دباغ",
                PhoneNumber = "+963966444555",
                PasswordHash = "AQAAAAIAAYagAAAAEANRgqEAvkfgcsO0Zjd60IIA+ispWX8yxdY2lyionPu+gyFDvN1RISG29ZIgmHKewQ==",
                ConcurrencyStamp = "d4aa48fe-1349-4786-9051-7892dfd6d97e",
                SecurityStamp = "d4aa48fe-1349-4786-9051-7892dfd6d97e"
            },
            new ApplicationUser
            {
                Id = 5,
                UserName = "mahmoud_plumber",
                NormalizedUserName = "MAHMOUD_PLUMBER",
                Email = "mahmoud@smartbooking.com",
                NormalizedEmail = "MAHMOUD@SMARTBOOKING.COM",
                EmailConfirmed = true,
                FirstName = "محمود",
                LastName = "صباغ",
                PhoneNumber = "+963988555666",
                PasswordHash = "AQAAAAIAAYagAAAAEANRgqEAvkfgcsO0Zjd60IIA+ispWX8yxdY2lyionPu+gyFDvN1RISG29ZIgmHKewQ==",
                ConcurrencyStamp = "e4aa48fe-1349-4786-9051-7892dfd6d97f",
                SecurityStamp = "e4aa48fe-1349-4786-9051-7892dfd6d97f"
            }
        );
    }
}
