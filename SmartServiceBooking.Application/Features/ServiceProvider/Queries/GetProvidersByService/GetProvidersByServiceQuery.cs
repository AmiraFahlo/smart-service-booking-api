namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetProvidersByService;

public record GetProvidersByServiceQuery(
    long ServiceId,
    string? District = null,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<Result<PagedList<ProviderLookupDto>>>;
