using System.Collections.Generic;
using System.Linq;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.SetProviderAvailability;

public class SetProviderAvailabilityCommandValidator : AbstractValidator<SetProviderAvailabilityCommand>
{
    public SetProviderAvailabilityCommandValidator()
    {
        RuleFor(x => x.ProviderId)
            .GreaterThan(0).WithMessage("Provider ID must be greater than 0.");

        RuleFor(x => x.Availabilities)
            .NotNull()
            .WithMessage("Availabilities list cannot be null.");

        RuleForEach(x => x.Availabilities)
            .ChildRules(availability =>
            {
                availability.RuleFor(a => a.DayOfWeek)
                    .IsInEnum().WithMessage("Invalid Day of Week.");

                availability.RuleFor(a => a.EndTime)
                    .GreaterThan(a => a.StartTime)
                    .WithMessage("End time must be after start time.");
            });

        // Ensure no duplicate days in the same request
        RuleFor(x => x.Availabilities)
            .Must(HaveUniqueDays)
            .WithMessage("Duplicate days are not allowed in the schedule.")
            .When(x => x.Availabilities != null);
    }

    private bool HaveUniqueDays(List<ProviderAvailabilityDto> availabilities)
    {
        var days = availabilities.Select(a => a.DayOfWeek).ToList();
        return days.Count == days.Distinct().Count();
    }
}
