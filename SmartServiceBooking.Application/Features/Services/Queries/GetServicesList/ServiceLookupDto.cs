namespace SmartServiceBooking.Application.Features.Services.Queries.GetServicesList;

public class ServiceLookupDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}
