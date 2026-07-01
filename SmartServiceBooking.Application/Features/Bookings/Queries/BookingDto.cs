namespace SmartServiceBooking.Application.Features.Bookings.Queries;

/// <summary>
/// Lightweight summary used in paginated lists (My Bookings / Provider Bookings).
/// </summary>
public class BookingDto
{
    public long Id { get; set; }
    public string BookingStatus { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime ScheduledStart { get; set; }
    public DateTime ScheduledEnd { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    // Service info
    public long ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;

    // Customer info
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    // Provider info
    public int ProviderId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
}
