using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Domain.Entities.Service;
using System.Threading;
using System.Threading.Tasks;

using SmartServiceBooking.Domain.Entities.Identity;

namespace SmartServiceBooking.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ServiceEntity> Services { get; }
    DbSet<Category> Categories { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<ServiceProvider> ServiceProviders { get; }
    DbSet<ProviderService> ProviderServices { get; }
    DbSet<ProviderAvailability> ProviderAvailabilities { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
