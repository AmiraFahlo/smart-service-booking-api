namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetProvidersList;

public record GetProvidersListQuery(
    string? District = null,
    int? CategoryId = null,
    string? SearchTerm = null,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<Result<PagedList<ServiceProviderProfileDto>>>;
