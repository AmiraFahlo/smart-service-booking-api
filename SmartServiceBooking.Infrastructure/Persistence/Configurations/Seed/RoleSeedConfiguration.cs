using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.Identity;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed;

public class RoleSeedConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(
            new ApplicationRole
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Administrator role with full access",
                ConcurrencyStamp = "b1b10204-5f8e-4a8b-980b-6a5124107661"
            },
            new ApplicationRole
            {
                Id = 2,
                Name = "Customer",
                NormalizedName = "CUSTOMER",
                Description = "Regular customer role",
                ConcurrencyStamp = "b2b10204-5f8e-4a8b-980b-6a5124107662"
            },
            new ApplicationRole
            {
                Id = 3,
                Name = "ServiceProvider",
                NormalizedName = "SERVICEPROVIDER",
                Description = "Service provider role",
                ConcurrencyStamp = "b3b10204-5f8e-4a8b-980b-6a5124107663"
            }
        );
    }
}
