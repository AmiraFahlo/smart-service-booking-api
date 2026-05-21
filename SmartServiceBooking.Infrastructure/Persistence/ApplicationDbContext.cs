using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Domain.Entities.Base;
using SmartServiceBooking.Domain.Entities.Booking;
using SmartServiceBooking.Domain.Entities.Identity;
using SmartServiceBooking.Domain.Entities.Interaction;
using SmartServiceBooking.Domain.Entities.Service;
using SmartServiceBooking.Domain.Entities.System;
using System.Reflection;
using SmartServiceBooking.Application.Common.Interfaces;

namespace SmartServiceBooking.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<
    ApplicationUser,
    ApplicationRole,
    int,
    IdentityUserClaim<int>,
    ApplicationUserRole,
    IdentityUserLogin<int>,
    IdentityRoleClaim<int>,
    ApplicationUserToken>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Service
    public DbSet<Category> Categories { get; set; }
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<SmartServiceBooking.Domain.Entities.Service.ServiceProvider> ServiceProviders { get; set; }
    public DbSet<ProviderService> ProviderServices { get; set; }
    public DbSet<ProviderAvailability> ProviderAvailabilities { get; set; }

    // Booking
    public DbSet<Booking> Bookings { get; set; }

    // Interaction
    public DbSet<Review> Reviews { get; set; }

    // Identity
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    // System
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<AppSetting> AppSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // ─────────────────────────────────────────────────────────────────────
        // GLOBAL QUERY FILTER — Soft Delete
        // ─────────────────────────────────────────────────────────────────────
        // Dynamically applies HasQueryFilter(e => !e.IsDeleted) to every entity
        // that implements ISoftDeletable. This replaces manual .Where(!IsDeleted)
        // in every handler and prevents accidental exposure of deleted records.
        //
        // HOW IT WORKS:
        //   1. Scans all entity types registered in the model.
        //   2. For each type that implements ISoftDeletable, invokes the generic
        //      SetSoftDeleteFilter<T>() method via Reflection.
        //   3. EF Core appends the filter to every generated SQL query automatically.
        //
        // TO BYPASS THE FILTER (Admin restore, Audit reports, migrations):
        //   context.Set<ServiceEntity>().IgnoreQueryFilters().Where(...)
        //
        // EXCLUDED ENTITIES (do NOT implement ISoftDeletable):
        //   - AuditLog     → audit records must always be visible
        //   - ApplicationUser → Identity manages its own deletion logic
        //   - AppSetting   → system configuration, never soft-deleted
        // ─────────────────────────────────────────────────────────────────────
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(SetSoftDeleteFilter),
                        BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(entityType.ClrType);

                method.Invoke(null, new object[] { modelBuilder });
            }
        }
    }

    /// <summary>
    /// Generic helper called via Reflection to register the soft-delete filter
    /// for a specific entity type. Must be static to allow MakeGenericMethod invocation.
    /// </summary>
    private static void SetSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, ISoftDeletable
    {
        modelBuilder.Entity<TEntity>()
            .HasQueryFilter(e => !e.IsDeleted);
    }

    // Audit fields (CreatedAt, UpdatedAt) are now handled automatically
    // by AuditableEntityInterceptor — no manual logic required here.
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);
}
