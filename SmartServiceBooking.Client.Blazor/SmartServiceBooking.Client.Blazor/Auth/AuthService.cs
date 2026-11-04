using System.Net.Http.Json;
using SmartServiceBooking.Shared.DTOs.Auth;

namespace SmartServiceBooking.Client.Blazor.Auth;

/// <summary>
/// Handles all authentication HTTP calls (Login, Logout, RefreshToken).
/// Delegates token storage and auth-state notification to CustomAuthStateProvider.
/// </summary>
public sealed class AuthService(
    IHttpClientFactory httpClientFactory,
    CustomAuthStateProvider authStateProvider,
    ILogger<AuthService> logger)
    : IAuthService
{
    // Uses the named HttpClient "ApiClient" (registered in Program.cs)
    private HttpClient Api => httpClientFactory.CreateClient("ApiClient");

    /// <inheritdoc />
    public async Task<AuthResponseDto?> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await Api.PostAsJsonAsync("api/auth/login", new
            {
                request.Email,
                request.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                logger.LogWarning("Login failed [{StatusCode}]: {Error}",
                    (int)response.StatusCode, error);
                return null;
            }

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (authResponse is null)
            {
                logger.LogError("Login response deserialization returned null.");
                return null;
            }

            // Store token + notify Blazor auth state
            await authStateProvider.NotifyUserAuthenticatedAsync(authResponse);

            logger.LogInformation("User {Email} logged in successfully.", request.Email);
            return authResponse;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during LoginAsync for {Email}.", request.Email);
            return null;
        }
    }

    /// <inheritdoc />
    public async Task LogoutAsync()
    {
        await authStateProvider.NotifyUserLoggedOutAsync();
        logger.LogInformation("User logged out.");
    }

    /// <inheritdoc />
    public async Task<string?> GetAccessTokenAsync()
        => await authStateProvider.GetStoredTokenAsync();
}
