using SmartServiceBooking.Domain.Entities.Service;

namespace SmartServiceBooking.Application.Common.Interfaces;

public interface IServiceProviderRepository
{
    Task<ServiceProvider?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ServiceProvider?> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<ServiceProvider> AddAsync(ServiceProvider serviceProvider, CancellationToken cancellationToken);
    Task UpdateAsync(ServiceProvider serviceProvider, CancellationToken cancellationToken);
    Task DeleteAsync(ServiceProvider serviceProvider, CancellationToken cancellationToken);
}
