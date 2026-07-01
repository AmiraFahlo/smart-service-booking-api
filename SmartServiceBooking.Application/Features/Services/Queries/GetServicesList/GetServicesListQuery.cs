namespace SmartServiceBooking.Application.Features.Services.Queries.GetServicesList;
public record GetServicesListQuery(
    string? SearchTerm = null,
    int? CategoryId = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    string? SortBy = "newest", // newest, price_asc, price_desc
    int PageNumber = 1,
    int PageSize = 10) : IRequest<Result<PagedList<ServiceLookupDto>>>;
