using System.ComponentModel.DataAnnotations;

namespace SmartServiceBooking.Shared.DTOs.Bookings;

/// <summary>
/// Request body for POST /api/bookings — matches CreateBookingCommand on the API side.
/// Used by the Blazor Booking Modal to submit a new booking.
/// </summary>
public class CreateBookingRequest
{
    [Required]
    public long ServiceId { get; set; }

    [Required]
    public string ProviderId { get; set; } = string.Empty;

    [Required]
    public long LocationId { get; set; }

    [Required]
    public DateTime ScheduledStart { get; set; }

    [Required]
    public DateTime ScheduledEnd { get; set; }

    public string? Notes { get; set; }
}
