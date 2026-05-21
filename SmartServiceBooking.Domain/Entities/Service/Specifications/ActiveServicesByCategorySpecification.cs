using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Domain.Entities.Service.Specifications;

public class ActiveServicesByCategorySpecification : Specification<ServiceEntity>
{
    public ActiveServicesByCategorySpecification(int categoryId)
        : base(s => s.CategoryId == categoryId)
    {
    }
}
