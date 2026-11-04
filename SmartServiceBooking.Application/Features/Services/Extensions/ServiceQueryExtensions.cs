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
            PriceInUSD = s.PriceInUSD,
            CategoryName = s.Category.Name
        });
    }

    public static IQueryable<ServiceEntity> ApplySearch(this IQueryable<ServiceEntity> query, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        var likeTerm = $"%{searchTerm.Trim()}%";
        return query.Where(s => EF.Functions.Like(s.Name, likeTerm) ||
                                (s.Description != null && EF.Functions.Like(s.Description, likeTerm)) ||
                                EF.Functions.Like(s.Category.Name, likeTerm));
    }

    public static IQueryable<ServiceEntity> ApplyFilters(this IQueryable<ServiceEntity> query, int? categoryId, decimal? minPrice, decimal? maxPrice)
    {
        if (categoryId.HasValue)
            query = query.Where(s => s.CategoryId == categoryId.Value);

        if (minPrice.HasValue)
            query = query.Where(s => s.BasePrice >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(s => s.BasePrice <= maxPrice.Value);

        return query;
    }

    public static IQueryable<ServiceEntity> ApplySorting(this IQueryable<ServiceEntity> query, string? sortBy)
    {
        return sortBy?.ToLower() switch
        {
            "price_asc" => query.OrderBy(s => s.BasePrice),
            "price_desc" => query.OrderByDescending(s => s.BasePrice),
            "name_asc" => query.OrderBy(s => s.Name),
            _ => query.OrderByDescending(s => s.CreatedAt)
        };
    }
}
