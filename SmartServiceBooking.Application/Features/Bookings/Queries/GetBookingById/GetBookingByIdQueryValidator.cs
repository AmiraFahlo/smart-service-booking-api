using FluentValidation;

namespace SmartServiceBooking.Application.Features.Bookings.Queries.GetBookingById;

public class GetBookingByIdQueryValidator : AbstractValidator<GetBookingByIdQuery>
{
    public GetBookingByIdQueryValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0)
            .WithMessage("Booking ID must be greater than 0.");
    }
}
