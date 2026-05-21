using MediatR;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Entities.Service;
using SmartServiceBooking.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace SmartServiceBooking.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IApplicationDbContext context,
    ICacheInvalidator cacheInvalidator)
    : IRequestHandler<CreateCategoryCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = Category.Create(
            name: request.Name,
            level: request.Level,
            parentId: request.ParentId,
            isActive: request.IsActive
        );

        categoryRepository.Add(category);

        await context.SaveChangesAsync(cancellationToken);

        await cacheInvalidator.InvalidateByTagAsync("categories-tag", cancellationToken);

        return Result.Success(category.Id);
    }
}
