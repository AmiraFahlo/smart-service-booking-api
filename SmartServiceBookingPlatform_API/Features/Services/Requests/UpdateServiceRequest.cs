using SmartServiceBooking.Application.Features.Services.Commands.UpdateService;
using SmartServiceBooking.Domain.Enums;

namespace SmartServiceBookingPlatform_API.Features.Services.Requests;

public record UpdateServiceRequest(
    int CategoryId,
    string Name,
    string? Description,
    decimal BasePrice,
    PriceUnit PriceUnit,
    bool IsActive)
{
    public UpdateServiceCommand ToCommand(long id) => new(
        id,
        CategoryId,
        Name,
        Description,
        BasePrice,
        PriceUnit,
        IsActive
    );
}
