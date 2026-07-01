using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartServiceBooking.Application.Features.Reviews.Commands.CreateReview;
using SmartServiceBooking.Application.Features.Reviews.Queries.GetProviderReviews;

namespace SmartServiceBookingPlatform_API.Controllers;

[Route("api/[controller]")]
public class ReviewsController : ApiControllerBase
{
    [HttpPost("/api/bookings/{bookingId:long}/reviews")]
    [Authorize]
    public async Task<IActionResult> CreateReview(long bookingId, [FromBody] CreateReviewRequest request)
    {
        var command = new CreateReviewCommand(bookingId, request.Rating, request.Comment);
        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            return HandleResult(result);
        }

        return Ok(new { ReviewId = result.Value });
    }

    [HttpGet("provider/{providerId:int}")]
    public async Task<IActionResult> GetProviderReviews(int providerId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetProviderReviewsQuery(providerId, pageNumber, pageSize);
        var result = await Sender.Send(query);

        return HandleResult(result);
    }
}

public class CreateReviewRequest
{
    public int Rating { get; set; }
    public string? Comment { get; set; }
}
