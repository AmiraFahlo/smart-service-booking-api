namespace SmartServiceBooking.Shared.DTOs.Auth;

/// <summary>
/// Response returned from POST /api/auth/login and POST /api/auth/refresh-token.
/// Mirrors the Application layer's AuthResponse record — kept here for Shared/Client use.
/// Fields: UserId, Email, FullName come from the API's AuthResponse record.
/// Roles are decoded client-side from the JWT Claims.
/// </summary>
public class AuthResponseDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
