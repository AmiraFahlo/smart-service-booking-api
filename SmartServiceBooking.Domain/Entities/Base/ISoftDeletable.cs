namespace SmartServiceBooking.Domain.Entities.Base;

/// <summary>
/// Marker interface for entities that support Soft Delete.
///
/// Any entity implementing this interface will automatically receive a
/// Global Query Filter in EF Core: WHERE IsDeleted = 0
///
/// To bypass the filter when needed (e.g., Admin restore, Audit reports):
///   context.Set&lt;T&gt;().IgnoreQueryFilters().Where(...)
///
/// Entities that should NOT implement this:
///   - AuditLog (audit records must never be hidden)
///   - ApplicationUser (Identity manages its own deletion logic)
///   - AppSetting (system configuration, never soft-deleted)
/// </summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; }
}
