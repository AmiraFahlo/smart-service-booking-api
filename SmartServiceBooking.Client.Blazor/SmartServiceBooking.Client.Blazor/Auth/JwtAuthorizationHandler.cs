using System.Net.Http.Headers;

namespace SmartServiceBooking.Client.Blazor.Auth;

/// <summary>
/// DelegatingHandler that automatically injects the JWT Bearer token
/// into every outgoing HTTP request from the Blazor Client to the API.
///
/// ⚠️ PRERENDERING SAFETY:
/// During the static HTML prerender phase, GetStoredTokenAsync() returns null
/// (CustomAuthStateProvider catches the InvalidOperationException internally).
/// In that case we simply forward the request without Authorization header —
/// which is safe because prerender-phase calls are for public/anonymous endpoints.
///
/// Registration: AddHttpMessageHandler<JwtAuthorizationHandler>() on the named HttpClient.
/// </summary>
public sealed class JwtAuthorizationHandler(
    CustomAuthStateProvider authStateProvider,
    ILogger<JwtAuthorizationHandler> logger)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Try to read stored token (returns null during Prerender — safe)
        var token = await authStateProvider.GetStoredTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            logger.LogDebug("JWT injected into request: {Method} {Uri}",
                request.Method, request.RequestUri);
        }

        var response = await base.SendAsync(request, cancellationToken);

        // Log 401 for visibility — RefreshToken flow can be added here later
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            logger.LogWarning("API returned 401 Unauthorized for {Method} {Uri}",
                request.Method, request.RequestUri);
        }

        return response;
    }
}
