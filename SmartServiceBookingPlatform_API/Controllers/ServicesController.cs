using SmartServiceBooking.Application.Features.Services.Commands.CreateService;
using SmartServiceBooking.Application.Features.Services.Commands.DeleteService;
using SmartServiceBooking.Application.Features.Services.Commands.UpdateService;
using SmartServiceBooking.Application.Features.Services.Queries.GetServiceById;
using SmartServiceBooking.Application.Features.Services.Queries.GetServicesList;
using SmartServiceBookingPlatform_API.Features.Services.Requests;

using Microsoft.AspNetCore.Authorization;
using SmartServiceBooking.Domain.Constants;

namespace SmartServiceBookingPlatform_API.Controllers;

[Authorize(Roles = $"{Roles.Provider},{Roles.Admin}")]
public class ServicesController : ApiControllerBase
{
    /// <summary>
    /// Returns a paginated, searchable list of active services.
    /// Response is cached for 5 minutes. Cache varies by all query parameters.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [OutputCache(PolicyName = CachePolicyNames.Services)]
    public async Task<IActionResult> GetServicesList([FromQuery] GetServicesListQuery query)
    {
        return HandleResult(await Sender.Send(query));
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetServiceById(long id)
    {
        return HandleResult(await Sender.Send(new GetServiceByIdQuery(id)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceRequest request)
    {
        var result = await Sender.Send(request.ToCommand());

        if (result.IsFailure)
            return HandleResult(result);

        return CreatedAtAction(
            actionName: nameof(GetServiceById),
            routeValues: new { id = result.Value },
            value: result.Value);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateService(long id, [FromBody] UpdateServiceRequest request)
    {
        return HandleResult(await Sender.Send(request.ToCommand(id)));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteService(long id)
    {
        return HandleResult(await Sender.Send(new DeleteServiceCommand(id)));
    }
}
