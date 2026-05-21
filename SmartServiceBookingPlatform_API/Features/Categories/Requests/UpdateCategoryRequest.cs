using SmartServiceBooking.Application.Features.Categories.Commands.UpdateCategory;

namespace SmartServiceBookingPlatform_API.Features.Categories.Requests;

public record UpdateCategoryRequest(
    string Name,
    bool IsActive,
    string? IconUrl)
{
    public UpdateCategoryCommand ToCommand(int id) => new(
        id,
        Name,
        IsActive,
        IconUrl
    );
}
