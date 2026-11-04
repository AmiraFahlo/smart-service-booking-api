namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.UpdateServiceProvider;

public record UpdateServiceProviderCommand(
    int Id,
    string? BusinessName,
    string? Bio,
    decimal CommissionRate,
    string? City = null,
    string? District = null,
    string? AddressDetails = null,
    double? Latitude = null,
    double? Longitude = null) : IRequest<Result>;
