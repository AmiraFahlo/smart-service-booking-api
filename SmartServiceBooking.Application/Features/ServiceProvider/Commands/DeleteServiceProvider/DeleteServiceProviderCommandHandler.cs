using MediatR;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Constants;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.DeleteServiceProvider;

public class DeleteServiceProviderCommandHandler(
    IApplicationDbContext context,
    IServiceProviderRepository providerRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeleteServiceProviderCommand, Result>
{
    public async Task<Result> Handle(DeleteServiceProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await providerRepository.GetByIdAsync(request.Id, cancellationToken);

        if (provider is null)
            return Result.Failure(ServiceProviderErrors.NotFound);

        // Check ownership: Admin can delete any, Provider can only delete their own
        if (!currentUserService.IsInRole(Roles.Admin))
        {
            var userId = currentUserService.ParsedUserId;
            if (userId == null || provider.UserId != userId.Value)
            {
                return Result.Failure(new Error(
                    "Auth.Forbidden", 
                    "You do not have permission to delete this service provider account.", 
                    ErrorType.Forbidden));
            }
        }

        provider.SoftDelete();

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
