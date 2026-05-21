namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetProvidersByService;

public record ProviderLookupDto
{
    public int Id { get; init; }
    public string? BusinessName { get; init; }
    public decimal Rating { get; init; }
    public int ReviewCount { get; init; }
    public bool IsVerified { get; init; }
    public decimal Price { get; init; }
}
