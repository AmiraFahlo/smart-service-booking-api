using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SmartServiceBooking.Domain.Entities.Base;

namespace SmartServiceBooking.Infrastructure.Persistence.Interceptors;

/// <summary>
/// EF Core SaveChanges Interceptor that automatically populates audit fields
/// (CreatedAt, UpdatedAt) on all AuditableEntity and AuditableEntity<TKey> entries.
///
/// Rules:
///   - Added state:    Sets CreatedAt = UtcNow (only if not already set — preserves Seed Data)
///   - Modified state: Sets UpdatedAt = UtcNow
///   - UpdatedBy/CreatedBy: Currently set to "System" — will be wired to ICurrentUserService in Phase 2
/// </summary>
public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            ApplyAuditFields(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            ApplyAuditFields(eventData.Context);
        }

        return base.SavingChanges(eventData, result);
    }

    private static void ApplyAuditFields(DbContext context)
    {
        var now = DateTime.UtcNow;

        // Handle AuditableEntity (non-generic: int Id)
        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                // SetCreatedAt internally checks if CreatedAt == default
                // so Seed Data with explicit DateTime.MinValue is preserved
                entry.Entity.SetCreatedAt(now);
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.SetUpdatedAt(now);
            }
        }

        // Handle AuditableEntity<TKey> (generic: long Id, Guid Id, etc.)
        // We use object entries + reflection on the marker interface pattern
        // because EF Core ChangeTracker.Entries<T> requires exact/base type
        foreach (var entry in context.ChangeTracker.Entries())
        {
            var entityType = entry.Entity.GetType();
            var baseType = entityType.BaseType;

            // Walk up the inheritance chain to find AuditableEntity<>
            while (baseType is not null)
            {
                if (baseType.IsGenericType &&
                    baseType.GetGenericTypeDefinition() == typeof(AuditableEntity<>))
                {
                    if (entry.State == EntityState.Added)
                    {
                        var setCreatedAt = baseType.GetMethod(
                            "SetCreatedAt",
                            System.Reflection.BindingFlags.Instance |
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Public);

                        setCreatedAt?.Invoke(entry.Entity, new object[] { now });
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        var setUpdatedAt = baseType.GetMethod(
                            "SetUpdatedAt",
                            System.Reflection.BindingFlags.Instance |
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Public);

                        setUpdatedAt?.Invoke(entry.Entity, new object[] { now });
                    }
                    break;
                }

                baseType = baseType.BaseType;
            }
        }
    }
}
