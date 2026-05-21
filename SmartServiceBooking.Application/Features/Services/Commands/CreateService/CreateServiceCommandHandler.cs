using MediatR;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Entities.Service;
using SmartServiceBooking.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

using SmartServiceBooking.Domain.Constants;

namespace SmartServiceBooking.Application.Features.Services.Commands.CreateService;

public class CreateServiceCommandHandler(
    IServiceRepository serviceRepository,
    IServiceProviderRepository providerRepository,
    ICurrentUserService currentUserService,
    IApplicationDbContext context,
    ICacheInvalidator cacheInvalidator)
    : IRequestHandler<CreateServiceCommand, Result<long>>
{
    public async Task<Result<long>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var providerResult = await GetAuthorizedProviderAsync(cancellationToken);
        if (providerResult.IsFailure)
        {
            return Result.Failure<long>(providerResult.Error);
        }

        var entity = ServiceEntity.Create(
            categoryId: request.CategoryId,
            name: request.Name,
            basePrice: request.BasePrice,
            priceUnit: request.PriceUnit,
            description: request.Description,
            isActive: request.IsActive
        );

        var providerService = ProviderService.Create(providerResult.Value.Id, 0);
        entity.ProviderServices.Add(providerService);

        serviceRepository.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        await cacheInvalidator.InvalidateByTagAsync("services-tag", cancellationToken);

        return Result.Success(entity.Id);
    }

    private async Task<Result<SmartServiceBooking.Domain.Entities.Service.ServiceProvider>> GetAuthorizedProviderAsync(CancellationToken cancellationToken)
    {
        var userId = currentUserService.ParsedUserId;
        if (userId == null)
        {
            return Result.Failure<SmartServiceBooking.Domain.Entities.Service.ServiceProvider>(
                new Error("Auth.Unauthorized", "User is not authenticated.", ErrorType.Unauthorized));
        }

        var provider = await providerRepository.GetByUserIdAsync(userId.Value, cancellationToken);
        if (provider == null)
        {
            return Result.Failure<SmartServiceBooking.Domain.Entities.Service.ServiceProvider>(
                new Error("Provider.NotFound", "Provider data not found for current user.", ErrorType.NotFound));
        }

        return Result.Success(provider);
    }
}
