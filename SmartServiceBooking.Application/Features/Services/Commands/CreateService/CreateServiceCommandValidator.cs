using FluentValidation;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Entities.Service.Specifications;

namespace SmartServiceBooking.Application.Features.Services.Commands.CreateService;

public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator(IServiceRepository serviceRepository)
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name must not be less than 3 characters.")
            .MustAsync(async (command, name, ct) => 
            {
                var spec = new ServiceByNameAndCategorySpecification(name, command.CategoryId);
                return !await serviceRepository.AnyAsync(spec, ct);
            })
            .WithMessage("A service with the same name already exists in this category.");

        RuleFor(v => v.BasePrice)
            .GreaterThan(0).WithMessage("BasePrice must be greater than 0.");
            
        RuleFor(v => v.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId is required.");
    }
}
