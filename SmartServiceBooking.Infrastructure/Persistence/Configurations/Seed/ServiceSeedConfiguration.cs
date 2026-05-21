using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.Service;
using SmartServiceBooking.Domain.Enums;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed;

public class ServiceSeedConfiguration : IEntityTypeConfiguration<Category>, IEntityTypeConfiguration<ServiceEntity>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasData(
            Category.Create(
                id: 1,
                name: "Home Maintenance",
                level: 0,
                parentId: null,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            ),
            Category.Create(
                id: 2,
                name: "Cleaning",
                level: 1,
                parentId: 1,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            ),
            Category.Create(
                id: 3,
                name: "Plumbing",
                level: 1,
                parentId: 1,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            )
        );
    }

    public void Configure(EntityTypeBuilder<ServiceEntity> builder)
    {
        builder.HasData(
            ServiceEntity.Create(
                id: 1,
                categoryId: 2, // Cleaning
                name: "Deep Cleaning",
                description: "Full deep cleaning service for apartments and villas.",
                basePrice: 50.00m,
                priceUnit: PriceUnit.Fixed,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            ),
            ServiceEntity.Create(
                id: 2,
                categoryId: 3, // Plumbing
                name: "Leak Repair",
                description: "Fixing water leaks in pipes or faucets.",
                basePrice: 30.00m,
                priceUnit: PriceUnit.Hourly,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            )
        );
    }
}
