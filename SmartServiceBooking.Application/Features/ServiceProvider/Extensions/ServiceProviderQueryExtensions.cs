using SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetProvidersByService;
using SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetServiceProviderById;
using SmartServiceBooking.Domain.Entities.Service;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Extensions;

public static class ServiceProviderQueryExtensions
{
    public static IQueryable<ServiceProviderProfileDto> MapToProfileDto(this IQueryable<Domain.Entities.Service.ServiceProvider> query)
    {
        return query.Select(p => new ServiceProviderProfileDto
        {
            Id = p.Id,
            UserId = p.UserId,
            BusinessName = p.BusinessName,
            Bio = p.Bio,
            Rating = p.Rating,
            ReviewCount = p.ReviewCount,
            IsVerified = p.IsVerified,
            Email = p.User != null ? p.User.Email : null,
            PhoneNumber = p.User != null ? p.User.PhoneNumber : null,
            FullName = p.User != null ? p.User.FirstName + " " + p.User.LastName : null
        });
    }

    public static IQueryable<ProviderLookupDto> MapToLookupDto(this IQueryable<ProviderService> query)
    {
        return query.Select(ps => new ProviderLookupDto
        {
            Id = ps.ProviderId,
            BusinessName = ps.Provider!.BusinessName,
            Rating = ps.Provider.Rating,
            ReviewCount = ps.Provider.ReviewCount,
            IsVerified = ps.Provider.IsVerified,
            Price = ps.CustomPrice ?? ps.Service!.BasePrice
        });
    }
}
