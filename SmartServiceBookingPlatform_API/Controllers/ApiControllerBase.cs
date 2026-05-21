using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBookingPlatform_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _sender;

    protected ISender Sender =>
        _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok();
        }

        return MapError(result.Error);
    }

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return MapError(result.Error);
    }

    private IActionResult MapError(Error error)
    {
        return error.Type switch
        {
            ErrorType.Validation => BadRequest(CreateProblemDetails("Validation Error", StatusCodes.Status400BadRequest, error)),
            ErrorType.NotFound => NotFound(CreateProblemDetails("Not Found", StatusCodes.Status404NotFound, error)),
            ErrorType.Conflict => Conflict(CreateProblemDetails("Conflict", StatusCodes.Status409Conflict, error)),
            ErrorType.Unauthorized => Unauthorized(CreateProblemDetails("Unauthorized", StatusCodes.Status401Unauthorized, error)),
            _ => BadRequest(CreateProblemDetails("Bad Request", StatusCodes.Status400BadRequest, error))
        };
    }

    private ProblemDetails CreateProblemDetails(string title, int status, Error error)
    {
        return new ProblemDetails
        {
            Title = title,
            Status = status,
            Detail = error.Description,
            Extensions = { { "code", error.Code } }
        };
    }
}
