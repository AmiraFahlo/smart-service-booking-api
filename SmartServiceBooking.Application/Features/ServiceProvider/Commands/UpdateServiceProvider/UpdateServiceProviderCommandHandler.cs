using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Constants;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.UpdateServiceProvider;

public class UpdateServiceProviderCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateServiceProviderCommand, Result>
{
    public async Task<Result> Handle(
        UpdateServiceProviderCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await context.ServiceProviders
            .FirstOrDefaultAsync(sp => sp.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            return Result.Failure(ServiceProviderErrors.NotFound);
        }

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

        return Result.Success();
    }
}
