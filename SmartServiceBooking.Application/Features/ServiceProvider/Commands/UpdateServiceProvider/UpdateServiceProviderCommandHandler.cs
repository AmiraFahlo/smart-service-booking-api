using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Constants;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.UpdateServiceProvider;

public class UpdateServiceProviderCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService,
    ICacheService cacheService,
    ICacheInvalidator cacheInvalidator)
    : IRequestHandler<UpdateServiceProviderCommand, Result>
{
    public async Task<Result> Handle(
        UpdateServiceProviderCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await context.ServiceProviders
            .FirstOrDefaultAsync(sp => sp.Id == request.Id, cancellationToken);

        if (entity is null)
            return Result.Failure(ServiceProviderErrors.NotFound);

        // 🛡️ Authorization Check (Ownership or Admin)
        var isAdmin = currentUserService.IsInRole(Roles.Admin);
        var isOwner = entity.UserId == currentUserService.ParsedUserId;

        if (!isAdmin && !isOwner)
        {
            return Result.Failure(new Error(
                "Auth.Forbidden",
                "You are not authorized to update this profile.",
                ErrorType.Forbidden));
        }

        // 💸 CommissionRate protection (Admin only)
        var commissionRate = isAdmin ? request.CommissionRate : entity.CommissionRate;

        entity.Update(
            businessName: request.BusinessName,
            bio: request.Bio,
            commissionRate: commissionRate,
            isVerified: entity.IsVerified
        );

        await context.SaveChangesAsync(cancellationToken);

        // 🧹 Cache Invalidation: clear stale data so clients see fresh info immediately
        // 1. Invalidate the provider's profile in Memory Cache
        await cacheService.RemoveAsync($"provider-availability-{entity.Id}", cancellationToken);

        // 2. Invalidate all "providers-by-service-*" lists via OutputCache tag
        await cacheInvalidator.InvalidateByTagAsync($"providers-by-service", cancellationToken);

        return Result.Success();
    }
}
