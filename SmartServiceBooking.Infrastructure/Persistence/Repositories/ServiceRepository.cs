using SmartServiceBooking.Domain.Entities.Service;
using SmartServiceBooking.Domain.Shared;
using SmartServiceBooking.Application.Features.Services;


namespace SmartServiceBooking.Infrastructure.Persistence.Repositories;

public class ServiceRepository(ApplicationDbContext context) : IServiceRepository
{
    public async Task<Result<ServiceEntity>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var service = await context.Services
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return service is not null 
            ? Result.Success(service) 
            : Result.Failure<ServiceEntity>(ServiceErrors.NotFound(id));
    }

    public async Task<bool> ExistsAsync(string name, long categoryId, long? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await context.Services
            .AnyAsync(s => s.Name == name 
                        && s.CategoryId == categoryId 
                        && (!excludeId.HasValue || s.Id != excludeId.Value), 
                cancellationToken);
    }

    public async Task<bool> AnyAsync(ISpecification<ServiceEntity> spec, CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator<ServiceEntity>
            .GetQuery(context.Services, spec)
            .AnyAsync(cancellationToken);
    }

    public async Task<int> CountAsync(ISpecification<ServiceEntity> spec, CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator<ServiceEntity>
            .GetQuery(context.Services, spec)
            .CountAsync(cancellationToken);
    }

    public void Add(ServiceEntity service)
    {
        context.Services.Add(service);
    }

    public void Update(ServiceEntity service)
    {
        context.Services.Update(service);
    }
}
