

namespace SmartServiceBookingPlatform_API.Controllers;

[Authorize(Roles = $"{Roles.Provider},{Roles.Admin}")]
public class ServiceProvidersController : ApiControllerBase
{
    [HttpGet("by-service/{serviceId:long}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByService(long serviceId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        return HandleResult(await Sender.Send(new GetProvidersByServiceQuery(serviceId, pageNumber, pageSize)));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        return HandleResult(await Sender.Send(new GetServiceProviderByIdQuery(id)));
    }

    [HttpPost]
    [Authorize] // Allows registered users (Customers) to create a provider profile
    public async Task<IActionResult> Create([FromBody] CreateServiceProviderCommand command)
    {
        return HandleResult(await Sender.Send(command));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceProviderRequest request)
    {
        return HandleResult(await Sender.Send(request.ToCommand(id)));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return HandleResult(await Sender.Send(new DeleteServiceProviderCommand(id)));
    }
}


