namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetServiceProviderById;
public class GetServiceProviderByIdQueryValidator : AbstractValidator<GetServiceProviderByIdQuery>
{
    public GetServiceProviderByIdQueryValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0)
            .WithMessage("Provider ID must be greater than 0.");
    }
}
