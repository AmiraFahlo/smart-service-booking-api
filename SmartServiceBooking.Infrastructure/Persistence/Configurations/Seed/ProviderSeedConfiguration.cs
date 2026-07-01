namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed;

public class ProviderSeedConfiguration : IEntityTypeConfiguration<DomainServiceProvider>, IEntityTypeConfiguration<ProviderService>
{
    public void Configure(EntityTypeBuilder<DomainServiceProvider> builder)
    {
        builder.HasData(
            DomainServiceProvider.Create(
                id: 1,
                userId: 1,
                businessName: "FixIt Fast Services",
                bio: "Professional maintenance services.",
                rating: 4.8m,
                reviewCount: 10,
                isVerified: true,
                commissionRate: 0.10m,
                createdAt: global::System.DateTime.MinValue
            ),
            DomainServiceProvider.Create(
                id: 2,
                userId: 2,
                businessName: "Mike's Plumbing",
                rating: 4.5m,
                createdAt: new global::System.DateTime(2025, 1, 1, 0, 0, 0, global::System.DateTimeKind.Utc)
            )
        );
    }

    public void Configure(EntityTypeBuilder<ProviderService> builder)
    {
        builder.HasData(
            ProviderService.Create(
                id: 1,
                providerId: 1,
                serviceId: 1,
                customPrice: 60.00m,
                isActive: true
            ),
            ProviderService.Create(
                id: 2,
                providerId: 2,
                serviceId: 2,
                customPrice: 35.00m,
                isActive: true
            )
        );
    }
}
