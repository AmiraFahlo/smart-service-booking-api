using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Entities.Service;
using SmartServiceBooking.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace SmartServiceBooking.Infrastructure.Persistence.Repositories;

public class ServiceProviderRepository : IServiceProviderRepository
{
    private readonly ApplicationDbContext _context;

    public ServiceProviderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SmartServiceBooking.Domain.Entities.Service.ServiceProvider?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.ServiceProviders
            .FirstOrDefaultAsync(sp => sp.Id == id, cancellationToken);
    }

    public async Task<SmartServiceBooking.Domain.Entities.Service.ServiceProvider?> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.ServiceProviders
            .FirstOrDefaultAsync(sp => sp.UserId == userId, cancellationToken);
    }

    public async Task<SmartServiceBooking.Domain.Entities.Service.ServiceProvider> AddAsync(SmartServiceBooking.Domain.Entities.Service.ServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        await _context.ServiceProviders.AddAsync(serviceProvider, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return serviceProvider;
    }

    public async Task UpdateAsync(SmartServiceBooking.Domain.Entities.Service.ServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        _context.ServiceProviders.Update(serviceProvider);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(SmartServiceBooking.Domain.Entities.Service.ServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        _context.ServiceProviders.Remove(serviceProvider);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
