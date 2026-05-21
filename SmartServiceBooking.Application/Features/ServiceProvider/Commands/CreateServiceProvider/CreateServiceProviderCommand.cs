namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.CreateServiceProvider;

public record CreateServiceProviderCommand(
    int UserId,
    string? BusinessName,
    string? Bio,
    decimal CommissionRate) : IRequest<Result<int>>;
