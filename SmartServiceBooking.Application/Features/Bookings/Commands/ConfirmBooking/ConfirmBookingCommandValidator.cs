using FluentValidation;

namespace SmartServiceBooking.Application.Features.Bookings.Commands.ConfirmBooking;

public class ConfirmBookingCommandValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0)
            .WithMessage("Booking ID must be greater than 0.");
    }
}
