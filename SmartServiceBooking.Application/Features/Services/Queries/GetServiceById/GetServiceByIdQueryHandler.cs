namespace SmartServiceBooking.Application.Features.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetServiceByIdQuery, Result<ServiceDetailDto>>
{
    public async Task<Result<ServiceDetailDto>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        // AsNoTracking: read-only query
        // Global Query Filter excludes soft-deleted services automatically
        // MapToDetailDto: single SQL query with ProviderCount sub-select (no N+1)
        var dto = await context.Services
            .AsNoTracking()
            .Where(s => s.Id == request.Id)
            .MapToDetailDto()
            .FirstOrDefaultAsync(cancellationToken);

        if (dto is null)
            return Result.Failure<ServiceDetailDto>(ServiceErrors.NotFound(request.Id));

        return Result.Success(dto);
    }
}
