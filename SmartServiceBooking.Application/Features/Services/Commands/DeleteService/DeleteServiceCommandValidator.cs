namespace SmartServiceBooking.Application.Features.Services.Commands.DeleteService;
public class DeleteServiceCommandValidator : AbstractValidator<DeleteServiceCommand>
{
    public DeleteServiceCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0)
            .WithMessage("A valid Service Id is required.");
    }
}
