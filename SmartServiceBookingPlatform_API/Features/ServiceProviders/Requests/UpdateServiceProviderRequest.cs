
namespace SmartServiceBookingPlatform_API.Features.ServiceProviders.Requests;

public record UpdateServiceProviderRequest(
    string? BusinessName,
    string? Bio,
    decimal CommissionRate,
    string? City = null,
    string? District = null,
    string? AddressDetails = null,
    double? Latitude = null,
    double? Longitude = null)
{
    public UpdateServiceProviderCommand ToCommand(int id) =>
        new(id, BusinessName, Bio, CommissionRate, City, District, AddressDetails, Latitude, Longitude);
}