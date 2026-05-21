using FluentValidation;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.CreateServiceProvider;

public class CreateServiceProviderCommandValidator : AbstractValidator<CreateServiceProviderCommand>
{
    public CreateServiceProviderCommandValidator()
    {
        RuleFor(v => v.BusinessName)
            .NotEmpty().WithMessage("Business name is required.")
            .MaximumLength(200).WithMessage("Business name must not exceed 200 characters.");

        RuleFor(v => v.Bio)
            .MaximumLength(1000).WithMessage("Bio must not exceed 1000 characters.");

        RuleFor(v => v.CommissionRate)
            .InclusiveBetween(0, 100).WithMessage("Commission rate must be between 0 and 100.");
    }
}
