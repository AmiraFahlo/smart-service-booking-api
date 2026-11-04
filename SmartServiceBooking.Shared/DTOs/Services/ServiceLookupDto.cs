namespace SmartServiceBooking.Shared.DTOs.Services
;
public class ServiceLookupDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? PriceInUSD { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string PriceFormattedSYP => $"{BasePrice:N0} ل.س";
    public string PriceFormattedUSD => $"${PriceInUSD ?? (BasePrice > 0 ? Math.Round(BasePrice / 15000m, 2) : 0):N2}";
}


