namespace SmartServiceBookingPlatform_API.Controllers;

[Authorize]
public class BookingsController : ApiControllerBase
{

    [HttpGet("my")]
    public async Task<IActionResult> GetMyBookings([FromQuery] GetMyBookingsQuery query)
    {
        return HandleResult(await Sender.Send(query));
    }

    [HttpGet("provider")]
    [Authorize(Roles = Roles.Provider)]
    public async Task<IActionResult> GetProviderBookings([FromQuery] GetProviderBookingsQuery query)
    {
        return HandleResult(await Sender.Send(query));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetBookingById(long id)
    {
        return HandleResult(await Sender.Send(new GetBookingByIdQuery(id)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
    {
        return HandleResult(await Sender.Send(command));
    }

    [HttpPut("{id:long}/confirm")]
    [Authorize(Roles = Roles.Provider)]
    public async Task<IActionResult> ConfirmBooking(long id)
    {
        return HandleResult(await Sender.Send(new ConfirmBookingCommand(id)));
    }

    [HttpPut("{id:long}/complete")]
    [Authorize(Roles = Roles.Provider)]
    public async Task<IActionResult> CompleteBooking(long id)
    {
        return HandleResult(await Sender.Send(new CompleteBookingCommand(id)));
    }

    [HttpPut("{id:long}/cancel")]
    public async Task<IActionResult> CancelBooking(long id)
    {
        return HandleResult(await Sender.Send(new CancelBookingCommand(id)));
    }
}
