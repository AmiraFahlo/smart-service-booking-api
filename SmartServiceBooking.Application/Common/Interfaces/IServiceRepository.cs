using SmartServiceBooking.Domain.Entities.Service;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Common.Interfaces;

public interface IServiceRepository
{
    Task<Result<ServiceEntity>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsAsync(string name, long categoryId, long? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(ISpecification<ServiceEntity> spec, CancellationToken cancellationToken = default);

    Task<int> CountAsync(ISpecification<ServiceEntity> spec, CancellationToken cancellationToken = default);
    
    void Add(ServiceEntity service);
    
    void Update(ServiceEntity service);
}
