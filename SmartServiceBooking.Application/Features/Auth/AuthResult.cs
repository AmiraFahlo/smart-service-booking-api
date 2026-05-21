using System;

namespace SmartServiceBooking.Application.Features.Auth;

public record AuthResult(
    string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiration);
