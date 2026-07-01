using FluentValidation;

namespace SmartServiceBooking.Application.Features.Bookings.Commands.CompleteBooking;

public class CompleteBookingCommandValidator : AbstractValidator<CompleteBookingCommand>
{
    public CompleteBookingCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0)
            .WithMessage("Booking ID must be greater than 0.");
    }
}
