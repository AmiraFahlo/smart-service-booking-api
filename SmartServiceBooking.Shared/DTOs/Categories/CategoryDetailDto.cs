namespace SmartServiceBooking.Shared.DTOs.Categories
;
public class CategoryDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int? ParentId { get; set; }
    public string? ParentName { get; set; }
    public string? IconUrl { get; set; }
    public bool IsActive { get; set; }
    public int ServiceCount { get; set; }
    public int SubCategoryCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}


