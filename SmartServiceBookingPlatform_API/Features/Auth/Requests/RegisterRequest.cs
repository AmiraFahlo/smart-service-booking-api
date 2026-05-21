using SmartServiceBooking.Application.Features.Auth.Commands.Register;

namespace SmartServiceBookingPlatform_API.Features.Auth.Requests;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName)
{
    public RegisterUserCommand ToCommand() => new(Email, Password, FirstName, LastName);
}
