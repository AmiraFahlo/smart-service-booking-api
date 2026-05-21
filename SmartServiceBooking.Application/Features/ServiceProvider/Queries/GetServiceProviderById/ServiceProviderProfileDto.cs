namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetServiceProviderById;

public record ServiceProviderProfileDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string? BusinessName { get; init; }
    public string? Bio { get; init; }
    public decimal Rating { get; init; }
    public int ReviewCount { get; init; }
    public bool IsVerified { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? FullName { get; init; }
}
