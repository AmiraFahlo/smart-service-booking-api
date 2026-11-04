namespace SmartServiceBooking.Shared.DTOs.ServiceProvider
;
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
    public string City { get; init; } = "حلب";
    public string? District { get; init; }
    public string? AddressDetails { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
}


