using System.Net.Http.Json;
using SmartServiceBooking.Shared.DTOs.Services;
using SmartServiceBooking.Shared.DTOs.Categories;

namespace SmartServiceBooking.Client.Blazor.Services;

/// <summary>
/// Handles all HTTP calls related to Services and Categories.
/// </summary>
public sealed class ServiceApiService(
    IHttpClientFactory httpClientFactory,
    ILogger<ServiceApiService> logger)
{
    private HttpClient Api => httpClientFactory.CreateClient("ApiClient");

    /// <summary>
    /// GET /api/services — returns all active services with optional search/filter params.
    /// This endpoint is [AllowAnonymous] on the API side.
    /// </summary>
    public async Task<List<ServiceDetailDto>> GetServicesAsync(
        string? searchTerm = null,
        int? categoryId = null,
        CancellationToken ct = default)
    {
        try
        {
            var query = BuildQuery(searchTerm, categoryId);
            var url = string.IsNullOrEmpty(query)
                ? "api/services"
                : $"api/services?{query}";

            var result = await Api.GetFromJsonAsync<List<ServiceDetailDto>>(url, ct);
            return result ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching services list.");
            return [];
        }
    }

    /// <summary>
    /// GET /api/categories — returns categories for filter dropdown.
    /// </summary>
    public async Task<List<CategoryLookupDto>> GetCategoriesAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await Api.GetFromJsonAsync<List<CategoryLookupDto>>("api/categories", ct);
            return result ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching categories list.");
            return [];
        }
    }

    // ──────────────────────────────────────────────────────────────────────
    private static string BuildQuery(string? searchTerm, int? categoryId)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(searchTerm))
            parts.Add($"searchTerm={Uri.EscapeDataString(searchTerm)}");
        if (categoryId.HasValue)
            parts.Add($"categoryId={categoryId}");
        return string.Join("&", parts);
    }
}
