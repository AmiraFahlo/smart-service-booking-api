namespace SmartServiceBooking.Shared.DTOs.ServiceProvider
;
public record ProviderLookupDto
{
    public int Id { get; init; }
    public string? BusinessName { get; init; }
    public decimal Rating { get; init; }
    public int ReviewCount { get; init; }
    public bool IsVerified { get; init; }
    public decimal Price { get; init; } // in SYP
    public decimal? PriceInUSD { get; init; } // in USD
    public string? City { get; init; }
    public string? District { get; init; }
    public string? AddressDetails { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public string PriceFormattedSYP => $"{Price:N0} ل.س";
    public string PriceFormattedUSD => $"${PriceInUSD ?? (Price > 0 ? Math.Round(Price / 15000m, 2) : 0):N2}";
}


