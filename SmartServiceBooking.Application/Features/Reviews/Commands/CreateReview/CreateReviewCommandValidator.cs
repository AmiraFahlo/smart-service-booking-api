using FluentValidation;
using SmartServiceBooking.Application.Features.Reviews.Commands.CreateReview;

namespace SmartServiceBooking.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5 stars.");

        RuleFor(x => x.Comment)
            .MaximumLength(1000)
            .WithMessage("Comment cannot exceed 1000 characters.");
    }
}
