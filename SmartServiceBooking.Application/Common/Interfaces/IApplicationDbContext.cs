using SmartServiceBooking.Domain.Entities.Booking;
using SmartServiceBooking.Domain.Entities.Interaction;

namespace SmartServiceBooking.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ServiceEntity> Services { get; }
    DbSet<Category> Categories { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<ServiceProvider> ServiceProviders { get; }
    DbSet<ProviderService> ProviderServices { get; }
    DbSet<ProviderAvailability> ProviderAvailabilities { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<Review> Reviews { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

