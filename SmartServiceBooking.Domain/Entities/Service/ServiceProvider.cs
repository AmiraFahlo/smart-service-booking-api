namespace SmartServiceBooking.Domain.Entities.Service;

public class ServiceProvider : AuditableEntity<int>, ISoftDeletable
{
    private ServiceProvider() { }

    public int UserId { get; private set; }
    public string? BusinessName { get; private set; }
    public string? Bio { get; private set; }
    public decimal Rating { get; private set; }
    public int ReviewCount { get; private set; }
    public bool IsVerified { get; private set; }
    public decimal CommissionRate { get; private set; }
    public ApplicationUser? User { get; private set; }
    public ICollection<ProviderService> Services { get; private set; } = new HashSet<ProviderService>();
    public ICollection<ProviderAvailability> Availabilities { get; private set; } = new HashSet<ProviderAvailability>();
    public ICollection<Booking.Booking> Bookings { get; private set; } = new HashSet<Booking.Booking>();

    public static ServiceProvider Create(
        int userId,
        string? businessName = null,
        string? bio = null,
        decimal rating = 0m,
        int reviewCount = 0,
        bool isVerified = false,
        decimal commissionRate = 0m,
        int? id = null,
        DateTime? createdAt = null)
    {
        var provider = new ServiceProvider
        {
            UserId = userId,
            BusinessName = businessName,
            Bio = bio,
            Rating = rating,
            ReviewCount = reviewCount,
            IsVerified = isVerified,
            CommissionRate = commissionRate
        };

        if (id.HasValue)
            provider.Id = id.Value;

        if (createdAt.HasValue)
            provider.SetAudit(createdAt.Value);

        return provider;
    }

    /// <summary>
    /// Updates the provider profile. CommissionRate and IsVerified are Admin-only fields.
    /// </summary>
    public void Update(string? businessName, string? bio, decimal commissionRate, bool isVerified)
    {
        BusinessName = businessName;
        Bio = bio;
        CommissionRate = commissionRate;
        IsVerified = isVerified;
    }

    /// <summary>
    /// Recalculates the provider's aggregate rating from the reviews table.
    /// Called by ReviewCreatedEventHandler via Domain Events — not for direct handler use.
    /// </summary>
    public void UpdateRating(decimal newAverageRating, int newReviewCount)
    {
        Rating = Math.Round(newAverageRating, 2);
        ReviewCount = newReviewCount;
    }



    /// <summary>
    /// Marks the provider as deleted (Soft Delete).
    /// </summary>
    public void SoftDelete()
    {
        IsDeleted = true;
    }

    /// <summary>
    /// Restores a soft-deleted provider to active state.
    /// </summary>
    public void Restore()
    {
        IsDeleted = false;
    }

    /// <summary>
    /// Atomically restores a soft-deleted provider and applies new profile data.
    /// Expresses a single Business Intent: "Re-register with updated info."
    /// CommissionRate is immutable unless explicitly provided (Admin-only privilege).
    /// </summary>
    public void RestoreAndUpdate(string? businessName, string? bio, decimal? newCommissionRate = null)
    {
        IsDeleted = false;
        BusinessName = businessName;
        Bio = bio;

        // CommissionRate is only updated if explicitly provided (Admin-only intent)
        if (newCommissionRate.HasValue)
            CommissionRate = newCommissionRate.Value;
    }
}
