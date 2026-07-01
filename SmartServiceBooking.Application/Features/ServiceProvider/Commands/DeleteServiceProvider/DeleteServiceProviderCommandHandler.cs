using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Constants;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.DeleteServiceProvider;

public class DeleteServiceProviderCommandHandler(
    IApplicationDbContext context,
    IServiceProviderRepository providerRepository,
    ICurrentUserService currentUserService,
    ICacheService cacheService,
    ICacheInvalidator cacheInvalidator)
    : IRequestHandler<DeleteServiceProviderCommand, Result>
{
    public async Task<Result> Handle(DeleteServiceProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await providerRepository.GetByIdAsync(request.Id, cancellationToken);

        if (provider is null)
            return Result.Failure(ServiceProviderErrors.NotFound);

        // 🛡️ Authorization Check: Admin can delete any, Provider can only delete their own
        if (!currentUserService.IsInRole(Roles.Admin))
        {
            var userId = currentUserService.ParsedUserId;
            if (userId is null || provider.UserId != userId.Value)
            {
                return Result.Failure(new Error(
                    "Auth.Forbidden",
                    "You do not have permission to delete this service provider account.",
                    ErrorType.Forbidden));
            }
        }

        provider.SoftDelete();
        await context.SaveChangesAsync(cancellationToken);

        // 🧹 Cache Invalidation: deleted provider must vanish from all caches immediately
        // 1. Remove availability schedule from Memory Cache
        await cacheService.RemoveAsync($"provider-availability-{provider.Id}", cancellationToken);

        // 2. Invalidate all paginated "providers-by-service-*" lists via OutputCache tag
        await cacheInvalidator.InvalidateByTagAsync("providers-by-service", cancellationToken);

        return Result.Success();
    }
}
