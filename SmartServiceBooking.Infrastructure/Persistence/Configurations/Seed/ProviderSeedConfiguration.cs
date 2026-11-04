namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed;

public class ProviderSeedConfiguration : IEntityTypeConfiguration<DomainServiceProvider>, IEntityTypeConfiguration<ProviderService>
{
    public void Configure(EntityTypeBuilder<DomainServiceProvider> builder)
    {
        builder.HasData(
            DomainServiceProvider.Create(
                id: 1,
                userId: 1,
                businessName: "م. عمار الحلبي - ورشة النور للطاقة والإنفيرتر",
                bio: "مهندس طاقة بديلة متخصص في كشف وتصليح أعطال الإنفيرترات وبرمجة أنظمة الطاقة الشمسية والبطاريات.",
                rating: 4.9m,
                reviewCount: 42,
                isVerified: true,
                commissionRate: 0.10m,
                city: "حلب",
                district: "الفرقان",
                addressDetails: "الفرقان - شارع الباسل قرب دوار الصخرة",
                latitude: 36.2085,
                longitude: 37.1175,
                createdAt: global::System.DateTime.MinValue
            ),
            DomainServiceProvider.Create(
                id: 2,
                userId: 2,
                businessName: "المعلم أبو أحمد - كهرباء منازل وأمبيرات",
                bio: "فني كهرباء منازل وفلل معتمد للكشف عن أعطال القواطع والشورت وتمديد شبكات الأمبير والبديل.",
                rating: 4.8m,
                reviewCount: 35,
                isVerified: true,
                commissionRate: 0.10m,
                city: "حلب",
                district: "الحمدانية",
                addressDetails: "الحمدانية - الحي الرابع جانب مدرسة الباسل",
                latitude: 36.1832,
                longitude: 37.1082,
                createdAt: global::System.DateTime.MinValue
            ),
            DomainServiceProvider.Create(
                id: 3,
                userId: 3,
                businessName: "مركز الشهباء لصيانة المولدات والميكانيك",
                bio: "ورشة متكاملة لصيانة محركات مولدات البنزين والديزل وميكانيك السيارات ولف الدينامو وإصلاح أعطال الإقلاع.",
                rating: 4.75m,
                reviewCount: 28,
                isVerified: true,
                commissionRate: 0.10m,
                city: "حلب",
                district: "السليمانية",
                addressDetails: "السليمانية - نزلة الكنيسة جانب مشفى الميدان",
                latitude: 36.2160,
                longitude: 37.1550,
                createdAt: global::System.DateTime.MinValue
            ),
            DomainServiceProvider.Create(
                id: 4,
                userId: 4,
                businessName: "مركز دباغ لخدمات التكييف والتبريد",
                bio: "صيانة أجهزة التكييف والبرادات المنزلية والتجارية وتعبئة غاز فريون أصلي وكشف تسريب دارات التبريد.",
                rating: 4.85m,
                reviewCount: 24,
                isVerified: true,
                commissionRate: 0.10m,
                city: "حلب",
                district: "الشهباء",
                addressDetails: "الشهباء الجديدة - شارع مدرسة الشروق",
                latitude: 36.2250,
                longitude: 37.1260,
                createdAt: global::System.DateTime.MinValue
            ),
            DomainServiceProvider.Create(
                id: 5,
                userId: 5,
                businessName: "ورشة الصباغ للصيانة والسباكة الحديثة",
                bio: "كشف تسربات المياه وصيانة المضخات وتوربينات السحب وتمديد شبكات المياه وبالونات الضغط الأوتوماتيكية.",
                rating: 4.7m,
                reviewCount: 19,
                isVerified: true,
                commissionRate: 0.10m,
                city: "حلب",
                district: "الموغامبو",
                addressDetails: "الموغامبو - خلف كلية الهندسة المدنية",
                latitude: 36.2150,
                longitude: 37.1250,
                createdAt: global::System.DateTime.MinValue
            )
        );
    }

    public void Configure(EntityTypeBuilder<ProviderService> builder)
    {
        builder.HasData(
            ProviderService.Create(
                id: 1,
                providerId: 1,
                serviceId: 1, // صيانة إنفيرتر
                customPrice: 150000.00m,
                customPriceInUSD: 10.00m,
                isActive: true
            ),
            ProviderService.Create(
                id: 2,
                providerId: 1,
                serviceId: 2, // تركيب طاقة شمسية
                customPrice: 320000.00m,
                customPriceInUSD: 21.00m,
                isActive: true
            ),
            ProviderService.Create(
                id: 3,
                providerId: 2,
                serviceId: 3, // كهرباء وتمديدات
                customPrice: 90000.00m,
                customPriceInUSD: 6.00m,
                isActive: true
            ),
            ProviderService.Create(
                id: 4,
                providerId: 3,
                serviceId: 4, // ميكانيك ومولدات
                customPrice: 180000.00m,
                customPriceInUSD: 12.00m,
                isActive: true
            ),
            ProviderService.Create(
                id: 5,
                providerId: 4,
                serviceId: 5, // تكييف
                customPrice: 220000.00m,
                customPriceInUSD: 14.50m,
                isActive: true
            ),
            ProviderService.Create(
                id: 6,
                providerId: 5,
                serviceId: 6, // سباكة ومضخات
                customPrice: 110000.00m,
                customPriceInUSD: 7.50m,
                isActive: true
            )
        );
    }
}
