using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Reviews.Commands.CreateReview;

public record CreateReviewCommand(
    long BookingId,
    int Rating,
    string? Comment) : IRequest<Result<long>>;
