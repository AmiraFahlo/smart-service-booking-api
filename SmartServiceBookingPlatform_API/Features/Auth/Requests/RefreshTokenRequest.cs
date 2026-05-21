using SmartServiceBooking.Application.Features.Auth.Commands.RefreshToken;

namespace SmartServiceBookingPlatform_API.Features.Auth.Requests;

public record RefreshTokenRequest(string Token)
{
    public RefreshTokenCommand ToCommand() => new(Token);
}
