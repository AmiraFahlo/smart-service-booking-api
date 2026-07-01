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

        return await context.SaveAndInvalidateAsync(category.Id, cacheInvalidator, "categories-tag", cancellationToken);
    }
}
