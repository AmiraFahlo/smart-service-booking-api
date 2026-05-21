namespace SmartServiceBooking.Domain.Entities.Booking;

public class Booking : AuditableEntity<long>, ISoftDeletable
{
    private Booking() { }

    public int CustomerId { get; private set; }
    public int ProviderId { get; private set; }
    public long ServiceId { get; private set; }
    public BookingStatus BookingStatus { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; } = PaymentStatus.Unpaid;
    public DateTime ScheduledStart { get; private set; }
    public DateTime ScheduledEnd { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string? Notes { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public byte[]? RowVersion { get; private set; } // Optimistic Concurrency

    public ApplicationUser? Customer { get; private set; }
    public ServiceProvider? Provider { get; private set; }
    public ServiceEntity? Service { get; private set; }

    public static Booking Create(
        int customerId,
        int providerId,
        long serviceId,
        BookingStatus bookingStatus,
        PaymentStatus paymentStatus,
        DateTime scheduledStart,
        DateTime scheduledEnd,
        decimal totalAmount,
        string? notes = null,
        long? id = null,
        DateTime? createdAt = null)
    {
        var booking = new Booking
        {
            CustomerId = customerId,
            ProviderId = providerId,
            ServiceId = serviceId,
            BookingStatus = bookingStatus,
            PaymentStatus = paymentStatus,
            ScheduledStart = scheduledStart,
            ScheduledEnd = scheduledEnd,
            TotalAmount = totalAmount,
            Notes = notes
        };

        if (id.HasValue)
            booking.Id = id.Value;

        if (createdAt.HasValue)
            booking.SetAudit(createdAt.Value);

        return booking;
    }
}
