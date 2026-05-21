
namespace SmartServiceBookingPlatform_API.Features.ServiceProviders.Requests;

public record UpdateServiceProviderRequest(
    string? BusinessName,
    string? Bio,
    decimal CommissionRate)
{
    public UpdateServiceProviderCommand ToCommand(int id) =>
        new(id, BusinessName, Bio, CommissionRate);
}