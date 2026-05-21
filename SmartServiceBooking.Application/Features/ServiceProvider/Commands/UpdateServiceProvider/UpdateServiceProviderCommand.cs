using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.UpdateServiceProvider;

public record UpdateServiceProviderCommand(
    int Id,
    string? BusinessName,
    string? Bio,
    decimal CommissionRate) : IRequest<Result>;
