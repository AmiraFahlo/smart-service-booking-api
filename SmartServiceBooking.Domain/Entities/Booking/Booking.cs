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

    /// <summary>
    /// Factory Method: Always creates a booking in Pending + Unpaid state.
    /// The Domain owns the initial state — no external caller can bypass this.
    /// </summary>
    public static Result<Booking> Create(
        int customerId,
        int providerId,
        long serviceId,
        DateTime scheduledStart,
        DateTime scheduledEnd,
        decimal totalAmount,
        string? notes = null)
    {
        if (scheduledStart < DateTime.UtcNow)
            return Result.Failure<Booking>(BookingErrors.PastDate);

        if (scheduledEnd <= scheduledStart)
            return Result.Failure<Booking>(BookingErrors.InvalidTimeRange);

        if (scheduledStart.Date != scheduledEnd.Date)
            return Result.Failure<Booking>(BookingErrors.CrossMidnight);

        return new Booking
        {
            CustomerId = customerId,
            ProviderId = providerId,
            ServiceId = serviceId,
            BookingStatus = BookingStatus.Pending,   // Always start Pending
            PaymentStatus = PaymentStatus.Unpaid,    // Always start Unpaid
            ScheduledStart = scheduledStart,
            ScheduledEnd = scheduledEnd,
            TotalAmount = totalAmount,
            Notes = notes
        };
    }

    // ─── State Machine ───────────────────────────────────────────────────────

    /// <summary>
    /// Provider accepts the booking. Only allowed from Pending state.
    /// </summary>
    public Result Confirm()
    {
        if (BookingStatus != BookingStatus.Pending)
            return Result.Failure(BookingErrors.InvalidTransition(BookingStatus, BookingStatus.Confirmed));

        BookingStatus = BookingStatus.Confirmed;
        return Result.Success();
    }

    /// <summary>
    /// Service was delivered. Only allowed from Confirmed state.
    /// </summary>
    public Result Complete()
    {
        if (BookingStatus != BookingStatus.Confirmed)
            return Result.Failure(BookingErrors.InvalidTransition(BookingStatus, BookingStatus.Completed));

        BookingStatus = BookingStatus.Completed;
        return Result.Success();
    }

    /// <summary>
    /// Cancels the booking. Not allowed if already Completed.
    /// </summary>
    public Result Cancel()
    {
        if (BookingStatus == BookingStatus.Completed)
            return Result.Failure(BookingErrors.CannotCancelCompleted);

        if (BookingStatus == BookingStatus.Cancelled)
            return Result.Failure(BookingErrors.AlreadyCancelled);

        BookingStatus = BookingStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        return Result.Success();
    }

    /// <summary>
    /// Marks the booking as paid.
    /// </summary>
    public void MarkAsPaid()
    {
        PaymentStatus = PaymentStatus.Paid;
    }
}
