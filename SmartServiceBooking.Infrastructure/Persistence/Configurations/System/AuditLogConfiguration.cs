using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartServiceBooking.Domain.Entities.Base;
using SmartServiceBooking.Domain.Entities.System;

namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.System;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs", "System");
        builder.Property(a => a.EntityName).HasMaxLength(100).IsRequired();
        builder.Property(a => a.EntityId).HasMaxLength(450).IsRequired();
        builder.Property(a => a.Action).HasMaxLength(50).IsRequired();
    }
}
public abstract class AuditableConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // نتحقق إذا كان الكيان يحتوي على خاصية IsDeleted
        if (typeof(AuditableEntity).IsAssignableFrom(typeof(TEntity)))
        {
            builder.HasQueryFilter(e => EF.Property<bool>(e, "IsDeleted") == false);
        }
    }
}