using FluentValidation;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.DeleteServiceProvider;

public class DeleteServiceProviderCommandValidator : AbstractValidator<DeleteServiceProviderCommand>
{
    public DeleteServiceProviderCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0)
            .WithMessage("Provider ID must be greater than 0.");
    }
}
