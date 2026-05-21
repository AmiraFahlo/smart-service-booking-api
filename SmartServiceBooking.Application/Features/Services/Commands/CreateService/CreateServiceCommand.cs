using MediatR;
using SmartServiceBooking.Domain.Enums;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Services.Commands.CreateService;

public record CreateServiceCommand(
    int CategoryId,
    string Name,
    string? Description,
    decimal BasePrice,
    PriceUnit PriceUnit,
    bool IsActive) : IRequest<Result<long>>;
