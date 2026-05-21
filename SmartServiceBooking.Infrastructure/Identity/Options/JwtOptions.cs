namespace SmartServiceBooking.Infrastructure.Identity.Options;

public class JwtOptions
{
    public const string SectionName = "JwtSettings";
    
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Secret { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; }
    public int RefreshTokenExpiryDays { get; init; }
}
