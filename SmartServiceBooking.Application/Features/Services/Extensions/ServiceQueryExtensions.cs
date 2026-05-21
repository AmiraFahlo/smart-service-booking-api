using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Features.Services.Queries.GetServiceById;
using SmartServiceBooking.Application.Features.Services.Queries.GetServicesList;
using SmartServiceBooking.Domain.Entities.Service;

namespace SmartServiceBooking.Application.Features.Services.Extensions;

public static class ServiceQueryExtensions
{
    public static IQueryable<ServiceDetailDto> MapToDetailDto(this IQueryable<ServiceEntity> query)
    {
        return query.Select(s => new ServiceDetailDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            BasePrice = s.BasePrice,
            PriceUnit = s.PriceUnit.ToString(),
            CategoryId = s.CategoryId,
            CategoryName = s.Category.Name,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            ProviderCount = s.ProviderServices.Count(ps => !ps.Provider!.IsDeleted && ps.IsActive)
        });
    }

    public static IQueryable<ServiceLookupDto> MapToLookupDto(this IQueryable<ServiceEntity> query)
    {
        return query.Select(s => new ServiceLookupDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            BasePrice = s.BasePrice,
            CategoryName = s.Category.Name
        });
    }
}
