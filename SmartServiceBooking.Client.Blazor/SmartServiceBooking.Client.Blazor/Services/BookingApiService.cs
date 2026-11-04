using System.Net.Http.Json;
using SmartServiceBooking.Shared.DTOs.Bookings;

namespace SmartServiceBooking.Client.Blazor.Services;

/// <summary>
/// Handles all HTTP calls related to Bookings.
/// All endpoints here require [Authorize] — JWT is injected by JwtAuthorizationHandler.
/// </summary>
public sealed class BookingApiService(
    IHttpClientFactory httpClientFactory,
    ILogger<BookingApiService> logger)
{
    private HttpClient Api => httpClientFactory.CreateClient("ApiClient");

    /// <summary>
    /// GET /api/bookings/my — returns the current authenticated user's bookings.
    /// </summary>
    public async Task<List<BookingDto>> GetMyBookingsAsync(
        string? status = null,
        CancellationToken ct = default)
    {
        try
        {
            var url = status is null ? "api/bookings/my" : $"api/bookings/my?status={status}";
            var result = await Api.GetFromJsonAsync<List<BookingDto>>(url, ct);
            return result ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching my bookings.");
            return [];
        }
    }

    /// <summary>
    /// GET /api/bookings/{id} — returns a single booking detail.
    /// </summary>
    public async Task<BookingDto?> GetBookingByIdAsync(long id, CancellationToken ct = default)
    {
        try
        {
            return await Api.GetFromJsonAsync<BookingDto>($"api/bookings/{id}", ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching booking {Id}.", id);
            return null;
        }
    }

    /// <summary>
    /// POST /api/bookings — creates a new booking.
    /// Returns true on success (HTTP 200/201), false on failure.
    /// </summary>
    public async Task<(bool Success, string? ErrorMessage)> CreateBookingAsync(
        CreateBookingRequest request,
        CancellationToken ct = default)
    {
        try
        {
            var response = await Api.PostAsJsonAsync("api/bookings", request, ct);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Booking created successfully for ServiceId={ServiceId}.", request.ServiceId);
                return (true, null);
            }

            var error = await response.Content.ReadAsStringAsync(ct);
            logger.LogWarning("Create booking failed [{Status}]: {Error}",
                (int)response.StatusCode, error);
            return (false, ParseApiError(error));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during CreateBookingAsync.");
            return (false, "حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى.");
        }
    }

    /// <summary>
    /// PUT /api/bookings/{id}/cancel — cancels an existing booking.
    /// </summary>
    public async Task<bool> CancelBookingAsync(long id, CancellationToken ct = default)
    {
        try
        {
            var response = await Api.PutAsync($"api/bookings/{id}/cancel", null, ct);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Booking {Id} cancelled successfully.", id);
                return true;
            }

            logger.LogWarning("Cancel booking {Id} failed [{Status}].",
                id, (int)response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during CancelBookingAsync for booking {Id}.", id);
            return false;
        }
    }

    // ──────────────────────────────────────────────────────────────────────
    private static string ParseApiError(string raw)
    {
        // Basic extraction — improve if API returns ProblemDetails JSON
        if (string.IsNullOrWhiteSpace(raw)) return "حدث خطأ من الخادم.";
        if (raw.Length > 200) return "حدث خطأ من الخادم.";
        return raw;
    }
}
