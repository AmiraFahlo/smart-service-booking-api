using System;

namespace SmartServiceBooking.Domain.Entities.Identity;

public class RefreshToken
{
    public int Id { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresOn { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? RevokedOn { get; private set; }
    public string? RevokedByIp { get; private set; }
    public string? ReplacedByToken { get; private set; }
    
    public int UserId { get; private set; }
    
    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
    public bool IsRevoked => RevokedOn != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    // EF Core constructor
    private RefreshToken() { }

    public static RefreshToken Create(string token, DateTime expiresOn, int userId)
    {
        return new RefreshToken
        {
            Token = token,
            ExpiresOn = expiresOn,
            CreatedOn = DateTime.UtcNow,
            UserId = userId
        };
    }

    public void Revoke(string? ipAddress = null, string? replacedByToken = null)
    {
        RevokedOn = DateTime.UtcNow;
        RevokedByIp = ipAddress;
        ReplacedByToken = replacedByToken;
    }
}
