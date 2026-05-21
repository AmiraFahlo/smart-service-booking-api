using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.System;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed;

public class AppSettingSeedConfiguration : IEntityTypeConfiguration<AppSetting>
{
    public void Configure(EntityTypeBuilder<AppSetting> builder)
    {
        builder.HasData(
            AppSetting.Create(id: 1, key: "TaxRate", value: "0.05", description: "5% Tax Rate"),
            AppSetting.Create(id: 2, key: "Currency", value: "USD", description: "Default Currency")
        );
    }
}
