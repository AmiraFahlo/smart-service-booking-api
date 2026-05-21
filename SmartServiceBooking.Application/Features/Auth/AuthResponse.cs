namespace SmartServiceBooking.Application.Features.Auth;

public record AuthResponse(
    int UserId,
    string Email,
    string FullName,
    string AccessToken,
    string RefreshToken);
