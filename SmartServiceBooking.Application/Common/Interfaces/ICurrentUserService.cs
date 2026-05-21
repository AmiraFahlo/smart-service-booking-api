namespace SmartServiceBooking.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    int? ParsedUserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    IEnumerable<string> Roles { get; }
    bool IsInRole(string role);
}
