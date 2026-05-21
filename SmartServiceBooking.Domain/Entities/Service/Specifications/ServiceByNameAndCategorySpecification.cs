using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Domain.Entities.Service.Specifications;

public class ServiceByNameAndCategorySpecification : Specification<ServiceEntity>
{
    public ServiceByNameAndCategorySpecification(string name, long categoryId, long? excludeId = null)
        : base(s => s.Name == name 
                 && s.CategoryId == categoryId 
                 && (!excludeId.HasValue || s.Id != excludeId.Value))
    {
    }
}
