using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SmartServiceBooking.Shared.DTOs.Auth;

namespace SmartServiceBooking.Client.Blazor.Auth;

/// <summary>
/// Custom Authentication State Provider for Blazor Server (InteractiveServer mode).
///
/// ⚠️ PRERENDERING SAFETY:
/// OnInitializedAsync is called TWICE in Blazor Server with Prerendering enabled:
///   1st pass → Static HTML phase: no JS/SignalR circuit yet → ProtectedSessionStorage throws InvalidOperationException.
///   2nd pass → Interactive circuit established → JS Interop available, storage works.
///
/// Solution: We catch InvalidOperationException in GetAuthenticationStateAsync and return
/// AnonymousUser silently. The auth state will be re-evaluated when the circuit connects.
/// </summary>
public sealed class CustomAuthStateProvider(
    ProtectedSessionStorage sessionStorage,
    ILogger<CustomAuthStateProvider> logger)
    : AuthenticationStateProvider
{
    private const string TokenKey = "auth_token";
    private const string RefreshTokenKey = "auth_refresh_token";

    private static readonly AuthenticationState AnonymousState =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    /// <summary>
    /// Called by Blazor's cascading auth state to determine if user is authenticated.
    /// Returns Anonymous during prerender phase (InvalidOperationException is expected and swallowed).
    /// </summary>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var tokenResult = await sessionStorage.GetAsync<string>(TokenKey);

            if (!tokenResult.Success || string.IsNullOrWhiteSpace(tokenResult.Value))
                return AnonymousState;

            var principal = ParseClaimsFromJwt(tokenResult.Value);

            // Check token expiry via the 'exp' claim
            var expClaim = principal.FindFirst("exp")?.Value;
            if (expClaim is not null)
            {
                var expiry = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim));
                if (expiry <= DateTimeOffset.UtcNow)
                {
                    logger.LogInformation("JWT token expired at {Expiry}. Clearing session.", expiry);
                    await sessionStorage.DeleteAsync(TokenKey);
                    return AnonymousState;
                }
            }

            return new AuthenticationState(principal);
        }
        catch (InvalidOperationException)
        {
            // ✅ Expected during Prerendering phase (no JS Interop circuit yet).
            // Return anonymous silently — will be re-evaluated after circuit connects.
            return AnonymousState;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error in GetAuthenticationStateAsync.");
            return AnonymousState;
        }
    }

    /// <summary>
    /// Called by AuthService after successful login.
    /// Stores token and notifies Blazor of the authenticated state.
    /// </summary>
    public async Task NotifyUserAuthenticatedAsync(AuthResponseDto authResponse)
    {
        await sessionStorage.SetAsync(TokenKey, authResponse.AccessToken);
        await sessionStorage.SetAsync(RefreshTokenKey, authResponse.RefreshToken);

        var principal = ParseClaimsFromJwt(authResponse.AccessToken);
        var authState = new AuthenticationState(principal);

        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    /// <summary>
    /// Called by AuthService on logout.
    /// Clears stored tokens and notifies Blazor of the anonymous state.
    /// </summary>
    public async Task NotifyUserLoggedOutAsync()
    {
        await sessionStorage.DeleteAsync(TokenKey);
        await sessionStorage.DeleteAsync(RefreshTokenKey);

        NotifyAuthenticationStateChanged(Task.FromResult(AnonymousState));
    }

    /// <summary>
    /// Reads the stored access token directly (used by JwtAuthorizationHandler).
    /// Returns null during Prerender phase.
    /// </summary>
    public async Task<string?> GetStoredTokenAsync()
    {
        try
        {
            var result = await sessionStorage.GetAsync<string>(TokenKey);
            return result.Success ? result.Value : null;
        }
        catch (InvalidOperationException)
        {
            return null; // Prerender phase — no storage available yet
        }
    }

    // ──────────────────────────────────────────────────────────────────────
    // Private Helpers
    // ──────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Parses JWT claims into a ClaimsPrincipal without signature validation.
    /// Signature is already validated by the API at login time.
    /// </summary>
    private static ClaimsPrincipal ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);

        // Normalize the 'role' claim to the standard ClaimTypes.Role
        var claims = token.Claims.Select(c =>
            c.Type == "role" ? new Claim(ClaimTypes.Role, c.Value) : c
        ).ToList();

        return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "jwt"));
    }
}
