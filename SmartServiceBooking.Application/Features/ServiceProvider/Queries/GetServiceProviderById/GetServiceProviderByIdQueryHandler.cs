namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetServiceProviderById;

public class GetServiceProviderByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetServiceProviderByIdQuery, Result<ServiceProviderProfileDto>>
{
    public async Task<Result<ServiceProviderProfileDto>> Handle(GetServiceProviderByIdQuery request, CancellationToken cancellationToken)
    {
        var provider = await context.ServiceProviders
            .AsNoTracking()
            .Include(p => p.User)
            .Where(p => p.Id == request.Id)
            .MapToProfileDto()
            .FirstOrDefaultAsync(cancellationToken);

        if (provider is null)
            return Result.Failure<ServiceProviderProfileDto>(ServiceProviderErrors.NotFound);

        return Result.Success(provider);
    }
}
