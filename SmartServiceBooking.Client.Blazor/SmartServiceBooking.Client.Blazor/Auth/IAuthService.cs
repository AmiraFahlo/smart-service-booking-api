using SmartServiceBooking.Shared.DTOs.Auth;

namespace SmartServiceBooking.Client.Blazor.Auth;

/// <summary>
/// Contract for all authentication operations in the Blazor Client.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Sends POST /api/auth/login, stores the token, and notifies auth state.
    /// Returns null on failure (invalid credentials / server error).
    /// </summary>
    Task<AuthResponseDto?> LoginAsync(LoginRequest request);

    /// <summary>
    /// Clears stored tokens and notifies auth state (logout).
    /// </summary>
    Task LogoutAsync();

    /// <summary>
    /// Reads the stored Access Token from ProtectedSessionStorage.
    /// Returns null during Prerender phase or when not authenticated.
    /// </summary>
    Task<string?> GetAccessTokenAsync();
}
