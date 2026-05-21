using SmartServiceBooking.Application.Features.Services.Commands.CreateService;
using SmartServiceBooking.Domain.Enums;

namespace SmartServiceBookingPlatform_API.Features.Services.Requests;

public record CreateServiceRequest(
    int CategoryId,
    string Name,
    string? Description,
    decimal BasePrice,
    PriceUnit PriceUnit,
    bool IsActive)
{
    public CreateServiceCommand ToCommand() => new(
        CategoryId,
        Name,
        Description,
        BasePrice,
        PriceUnit,
        IsActive
    );
}
