using SmartServiceBooking.Application.Features.Auth.Commands.Login;

namespace SmartServiceBookingPlatform_API.Features.Auth.Requests;

public record LoginRequest(
    string Email,
    string Password)
{
    public LoginUserCommand ToCommand() => new(Email, Password);
}
