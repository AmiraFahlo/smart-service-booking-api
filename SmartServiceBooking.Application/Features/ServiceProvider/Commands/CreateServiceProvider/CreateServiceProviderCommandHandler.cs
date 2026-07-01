namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.CreateServiceProvider;

public class CreateServiceProviderCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<CreateServiceProviderCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        CreateServiceProviderCommand request,
        CancellationToken cancellationToken)
    {
        // 🛡️ Securely determine target UserId
        var isAdmin = currentUserService.IsInRole(Roles.Admin);
        var targetUserId = (isAdmin && request.UserId > 0)
            ? request.UserId
            : currentUserService.ParsedUserId ?? 0;

        if (targetUserId == 0)
            return Result.Failure<int>(new Error("Auth.InvalidUser", "User identification failed.", ErrorType.Unauthorized));

        // 1. Lookup by Id (= UserId via Shared PK) → uses Clustered Index for O(log n) performance
        var existingProvider = await context.ServiceProviders
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(sp => sp.Id == targetUserId, cancellationToken);

        // 🔄 Restore Pattern: soft-deleted → reactivate atomically instead of rejecting
        if (existingProvider is not null)
        {
            if (!existingProvider.IsDeleted)
                return Result.Failure<int>(ServiceProviderErrors.AlreadyExists);

            // Single Domain method expressing one Business Intent (DDD)
            existingProvider.RestoreAndUpdate(
                businessName: request.BusinessName,
                bio: request.Bio,
                newCommissionRate: isAdmin ? request.CommissionRate : null);

            // 🔒 Concurrency guard: handles race conditions under high load
            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result.Failure<int>(new Error(
                    "ServiceProvider.ConcurrencyConflict",
                    "The provider profile was modified by another request. Please try again.",
                    ErrorType.Conflict));
            }

            return Result.Success(existingProvider.Id);
        }

        // 💸 CommissionRate protection: Only Admin can set initial commission
        var commissionRate = isAdmin ? request.CommissionRate : 0m;

        // 2. Create fresh entity using Domain Factory
        var entity = Domain.Entities.Service.ServiceProvider.Create(
            userId: targetUserId,
            businessName: request.BusinessName,
            bio: request.Bio,
            commissionRate: commissionRate,
            id: targetUserId // 🛠️ Shared Primary Key (ServiceProvider.Id = UserId)
        );

        context.ServiceProviders.Add(entity);

        // 🔒 Concurrency guard: handles duplicate insert race condition
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("PRIMARY KEY") == true)
        {
            return Result.Failure<int>(ServiceProviderErrors.AlreadyExists);
        }

        return Result.Success(entity.Id);
    }
}
