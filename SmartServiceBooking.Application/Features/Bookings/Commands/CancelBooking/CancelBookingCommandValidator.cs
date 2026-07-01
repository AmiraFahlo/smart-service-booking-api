namespace SmartServiceBooking.Application.Features.Bookings.Commands.CancelBooking;

public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0)
            .WithMessage("Booking ID must be greater than 0.");
    }
}
