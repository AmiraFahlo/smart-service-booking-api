namespace SmartServiceBookingPlatform_API.Controllers;

public class AuthController : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        return HandleResult(await Sender.Send(request.ToCommand()));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        return HandleResult(await Sender.Send(request.ToCommand()));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        return HandleResult(await Sender.Send(request.ToCommand()));
    }
}
