using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Domain.Entities.Service.Specifications;

public class CategoryByNameSpecification : Specification<Category>
{
    public CategoryByNameSpecification(string name, int? parentId, int? excludeId = null)
        : base(c => c.Name == name 
                 && c.ParentId == parentId 
                 && (!excludeId.HasValue || c.Id != excludeId.Value))
    {
    }
}
