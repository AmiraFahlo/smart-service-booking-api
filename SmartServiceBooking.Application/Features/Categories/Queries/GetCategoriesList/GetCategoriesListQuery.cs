using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Categories.Queries.GetCategoriesList;

public record GetCategoriesListQuery(
    int? ParentId = null,
    bool? IsActive = null,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<Result<PagedList<CategoryLookupDto>>>;
