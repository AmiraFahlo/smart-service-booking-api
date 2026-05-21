using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    int Level,
    int? ParentId,
    bool IsActive,
    string? IconUrl) : IRequest<Result<int>>;
