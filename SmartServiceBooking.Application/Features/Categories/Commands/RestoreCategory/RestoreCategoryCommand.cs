using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.Categories.Commands.RestoreCategory;

public record RestoreCategoryCommand(int Id) : IRequest<Result>;
