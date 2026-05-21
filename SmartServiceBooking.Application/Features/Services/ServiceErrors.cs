using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Services;

/// <summary>
/// Centralized, strongly-typed error definitions for the Services domain.
/// Handlers reference these constants instead of inline strings — prevents typos,
/// enables easier testing, and provides a single source of truth for all error codes.
/// </summary>
public static class ServiceErrors
{
    /// <summary>
    /// Returned when a service with the requested Id does not exist (or is soft-deleted).
    /// </summary>
    public static Error NotFound(long id) =>
        new("Service.NotFound", $"Service with Id '{id}' was not found.", ErrorType.NotFound);

    /// <summary>
    /// Returned when attempting to delete an already soft-deleted service.
    /// </summary>
    public static Error AlreadyDeleted(long id) =>
        new("Service.NotFound", $"Service with Id '{id}' was not found or is already deleted.", ErrorType.NotFound);

    public static Error DuplicateName(string name) =>
        new("Service.DuplicateName", $"A service named '{name}' already exists in this category.", ErrorType.Conflict);
}
