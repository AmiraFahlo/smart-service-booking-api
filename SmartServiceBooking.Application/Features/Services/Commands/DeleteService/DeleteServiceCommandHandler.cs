using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;
using SmartServiceBooking.Domain.Entities.Service;

using SmartServiceBooking.Domain.Constants;

namespace SmartServiceBooking.Application.Features.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler(
    IApplicationDbContext context,
    IServiceProviderRepository providerRepository,
    ICurrentUserService currentUserService,
    ICacheInvalidator cacheInvalidator)
    : IRequestHandler<DeleteServiceCommand, Result>
{
    public async Task<Result> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await context.Services
            .Include(s => s.ProviderServices)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (service is null)
            return Result.Failure(ServiceErrors.AlreadyDeleted(request.Id));

        var ownershipResult = await CheckOwnershipAsync(service, cancellationToken);
        if (ownershipResult.IsFailure)
            return ownershipResult;

        service.SoftDelete();

        await context.SaveChangesAsync(cancellationToken);

        await cacheInvalidator.InvalidateByTagAsync("services-tag", cancellationToken);

        return Result.Success();
    }

    private async Task<Result> CheckOwnershipAsync(ServiceEntity service, CancellationToken cancellationToken)
    {
        if (currentUserService.IsInRole(Roles.Admin))
            return Result.Success();

        var userId = currentUserService.ParsedUserId;
        if (userId == null)
            return Result.Failure(new Error("Auth.Unauthorized", "User is not authenticated.", ErrorType.Unauthorized));

        var provider = await providerRepository.GetByUserIdAsync(userId.Value, cancellationToken);
        if (provider == null || !service.ProviderServices.Any(ps => ps.ProviderId == provider.Id))
        {
            return Result.Failure(new Error("Auth.Forbidden", "You do not have permission to delete this service.", ErrorType.Forbidden));
        }

        return Result.Success();
    }
}
