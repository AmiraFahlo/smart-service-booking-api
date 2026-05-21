namespace SmartServiceBooking.Application.Features.Categories.Queries.GetCategoriesList;

public class CategoryLookupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int? ParentId { get; set; }
    public string? ParentName { get; set; }
    public bool IsActive { get; set; }
    public int ServiceCount { get; set; }
}
