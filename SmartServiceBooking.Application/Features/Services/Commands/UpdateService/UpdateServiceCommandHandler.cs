namespace SmartServiceBooking.Application.Features.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler(
    IServiceProviderRepository providerRepository,
    ICurrentUserService currentUserService,
    IApplicationDbContext context,
    ICacheInvalidator cacheInvalidator)
    : IRequestHandler<UpdateServiceCommand, Result>
{
    public async Task<Result> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await context.Services
            .Include(s => s.ProviderServices)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (service is null)
            return Result.Failure(new Error("Service.NotFound", "Service not found.", ErrorType.NotFound));

        var ownershipResult = await CheckOwnershipAsync(service, cancellationToken);
        if (ownershipResult.IsFailure)
            return ownershipResult;

        service.Update(
            categoryId: request.CategoryId,
            name: request.Name,
            basePrice: request.BasePrice,
            priceUnit: request.PriceUnit,
            description: request.Description,
            isActive: request.IsActive
        );

        return await context.SaveAndInvalidateAsync(cacheInvalidator, "services-tag", cancellationToken);
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
            return Result.Failure(new Error("Auth.Forbidden", "You do not have permission to update this service.", ErrorType.Forbidden));
        }

        return Result.Success();
    }
}
