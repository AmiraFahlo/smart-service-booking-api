using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Categories;

/// <summary>
/// Centralized, strongly-typed error definitions for the Categories domain.
/// Handlers reference these constants instead of inline strings — prevents typos,
/// enables easier testing, and provides a single source of truth for all error codes.
/// </summary>
public static class CategoryErrors
{
    /// <summary>
    /// Returned when a category with the requested Id does not exist (or is soft-deleted).
    /// </summary>
    public static Error NotFound(int id) =>
        new("Category.NotFound", $"Category with Id '{id}' was not found.", ErrorType.NotFound);

    /// <summary>
    /// Returned when a restore operation finds no deleted record matching the Id.
    /// </summary>
    public static Error NotFoundDeleted(int id) =>
        new("Category.NotFound", $"No deleted category with Id '{id}' was found to restore.", ErrorType.NotFound);

    /// <summary>
    /// Returned when a category name already exists under the same parent.
    /// Enforces Unique(Name + ParentId) business rule.
    /// </summary>
    public static Error DuplicateName(string name) =>
        new("Category.DuplicateName", $"A category named '{name}' already exists under the same parent.", ErrorType.Conflict);

    /// <summary>
    /// Returned when attempting to delete a category that still has active services linked to it.
    /// Prevents orphaned services — a core data integrity rule.
    /// </summary>
    public static Error HasActiveServices(string categoryName, int count) =>
        new("Category.HasActiveServices",
            $"Cannot delete '{categoryName}' because it has {count} active service(s) linked to it. " +
            "Reassign or delete the services first.", ErrorType.Conflict);
}
