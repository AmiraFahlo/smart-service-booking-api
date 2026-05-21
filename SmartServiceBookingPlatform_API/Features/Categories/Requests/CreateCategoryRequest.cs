using SmartServiceBooking.Application.Features.Categories.Commands.CreateCategory;

namespace SmartServiceBookingPlatform_API.Features.Categories.Requests;

public record CreateCategoryRequest(
    string Name,
    int Level,
    int? ParentId,
    bool IsActive,
    string? IconUrl)
{
    public CreateCategoryCommand ToCommand() => new(
        Name,
        Level,
        ParentId,
        IsActive,
        IconUrl
    );
}
