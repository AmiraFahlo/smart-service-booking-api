namespace SmartServiceBooking.Domain.Enums;

public enum BookingStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled
}

public enum PriceUnit
{
    Fixed,
    Hourly
}

public enum PaymentStatus
{
    Unpaid,
    Paid
}
