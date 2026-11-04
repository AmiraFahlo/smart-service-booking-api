namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.CreateServiceProvider;

public record CreateServiceProviderCommand(
    int UserId,
    string? BusinessName,
    string? Bio,
    decimal CommissionRate,
    string? City = "حلب",
    string? District = null,
    string? AddressDetails = null,
    double? Latitude = null,
    double? Longitude = null) : IRequest<Result<int>>;
