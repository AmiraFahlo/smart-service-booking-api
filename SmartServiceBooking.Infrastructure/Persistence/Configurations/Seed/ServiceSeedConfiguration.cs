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
                name: "طاقة شمسية وبديلة",
                level: 0,
                parentId: null,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            ),
            Category.Create(
                id: 2,
                name: "كهرباء وتمديدات",
                level: 0,
                parentId: null,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            ),
            Category.Create(
                id: 3,
                name: "ميكانيك ومولدات",
                level: 0,
                parentId: null,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            ),
            Category.Create(
                id: 4,
                name: "صيانة وتكييف",
                level: 0,
                parentId: null,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            ),
            Category.Create(
                id: 5,
                name: "سباكة وصحية",
                level: 0,
                parentId: null,
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
                categoryId: 1, // طاقة شمسية وبديلة
                name: "صيانة وفحص إنفيرتر (Inverter)",
                description: "كشف أعطال الشحن والتحويل وتبديل موسفتات وبرمجة أنظمة الطاقة والبطاريات.",
                basePrice: 150000.00m,
                priceUnit: PriceUnit.Fixed,
                priceInUSD: 10.00m,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            ),
            ServiceEntity.Create(
                id: 2,
                categoryId: 1, // طاقة شمسية وبديلة
                name: "تركيب وضبط منظومة طاقة شمسية",
                description: "تركيب وتثبيت القواعد والألواح الشمسية مع التوصيل الدقيق وحماية القواطع.",
                basePrice: 300000.00m,
                priceUnit: PriceUnit.Fixed,
                priceInUSD: 20.00m,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            ),
            ServiceEntity.Create(
                id: 3,
                categoryId: 2, // كهرباء وتمديدات
                name: "صيانة قواطع وشبكة كهرباء منزلية",
                description: "كشف أعطال الشورت والتسريب وفحص لوحات التوزيع وتمديد خطوط أمبير وبديل.",
                basePrice: 90000.00m,
                priceUnit: PriceUnit.Hourly,
                priceInUSD: 6.00m,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            ),
            ServiceEntity.Create(
                id: 4,
                categoryId: 3, // ميكانيك ومولدات
                name: "صيانة وضبط محركات المولدات المنزلية",
                description: "صيانة كربيراتير وتبديل فلاتر وفحص مارش ودينامو المولدات ومشاكل الكهرباء.",
                basePrice: 180000.00m,
                priceUnit: PriceUnit.Fixed,
                priceInUSD: 12.00m,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            ),
            ServiceEntity.Create(
                id: 5,
                categoryId: 4, // صيانة وتكييف
                name: "صيانة مكيف وتعبئة غاز تبريد",
                description: "تنظيف الفلاتر وفحص الكومبريسور وتعبئة غاز تبريد أصلي للمكيفات والبرادات.",
                basePrice: 225000.00m,
                priceUnit: PriceUnit.Fixed,
                priceInUSD: 15.00m,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            ),
            ServiceEntity.Create(
                id: 6,
                categoryId: 5, // سباكة وصحية
                name: "صيانة مضخات المياه وبالونات الضغط",
                description: "إصلاح توربين المضخة وتبديل مفتاح الأوتوماتيك والبالون وفحص تسريب المياه.",
                basePrice: 120000.00m,
                priceUnit: PriceUnit.Fixed,
                priceInUSD: 8.00m,
                isActive: true,
                createdAt: global::System.DateTime.MinValue
            )
        );
    }
}
