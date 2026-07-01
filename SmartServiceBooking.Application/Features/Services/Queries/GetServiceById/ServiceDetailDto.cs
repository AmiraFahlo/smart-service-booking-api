namespace SmartServiceBooking.Application.Features.Services.Queries.GetServiceById;

public class ServiceDetailDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public string PriceUnit { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    /// <summary>
    /// Count of active, non-deleted service providers linked to this service.
    /// Computed via SQL projection in a single query.
    /// </summary>
    public int ProviderCount { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
