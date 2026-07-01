using FluentValidation;

namespace SmartServiceBooking.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(v => v.ProviderId)
            .GreaterThan(0)
            .WithMessage("Provider ID must be greater than 0.");

        RuleFor(v => v.ServiceId)
            .GreaterThan(0)
            .WithMessage("Service ID must be greater than 0.");

        RuleFor(v => v.ScheduledStart)
            .NotEmpty()
            .WithMessage("Scheduled start time is required.")
            .Must(start => start >= DateTime.UtcNow.AddMinutes(-5))
            .WithMessage("Scheduled start time cannot be in the past.");

        RuleFor(v => v.ScheduledEnd)
            .NotEmpty()
            .WithMessage("Scheduled end time is required.")
            .GreaterThan(v => v.ScheduledStart)
            .WithMessage("Scheduled end time must be after the start time.");

        RuleFor(v => v)
            .Must(v => v.ScheduledStart.Date == v.ScheduledEnd.Date)
            .WithMessage("The booking must start and end on the same calendar day.");
    }
}
