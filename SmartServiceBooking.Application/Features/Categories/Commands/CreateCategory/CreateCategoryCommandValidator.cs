using FluentValidation;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Entities.Service.Specifications;

namespace SmartServiceBooking.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MinimumLength(3).WithMessage("Category name must be at least 3 characters.")
            .MustAsync(async (command, name, ct) => 
            {
                var spec = new CategoryByNameSpecification(name, command.ParentId);
                return !await categoryRepository.AnyAsync(spec, ct);
            })
            .WithMessage("A category with the same name already exists under this parent.");

        RuleFor(v => v.Level)
            .GreaterThan(0).WithMessage("Level must be greater than 0.");
    }
}
