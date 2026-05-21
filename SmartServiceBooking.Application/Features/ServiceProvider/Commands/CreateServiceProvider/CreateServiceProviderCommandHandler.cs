using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Constants;
using SmartServiceBooking.Domain.Shared;

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
        {
            return Result.Failure<int>(new Error("Auth.InvalidUser", "User identification failed.", ErrorType.Unauthorized));
        }

        // 1. Check if user already has a provider profile
        var exists = await context.ServiceProviders
            .AnyAsync(sp => sp.UserId == targetUserId, cancellationToken);

        if (exists)
        {
            return Result.Failure<int>(ServiceProviderErrors.AlreadyExists);
        }

        // 💸 CommissionRate protection: Only Admin can set initial commission
        var commissionRate = isAdmin ? request.CommissionRate : 0m;

        // 2. Create the entity using the Domain Factory
        var entity = Domain.Entities.Service.ServiceProvider.Create(
            userId: targetUserId,
            businessName: request.BusinessName,
            bio: request.Bio,
            commissionRate: commissionRate,
            id: targetUserId // 🛠️ Fix: Shared Primary Key (ServiceProvider.Id = UserId)
        );

        // 3. Persist
        context.ServiceProviders.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.Id);
    }
}
