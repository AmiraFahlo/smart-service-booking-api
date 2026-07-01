namespace SmartServiceBooking.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IApplicationDbContext context,
    ICacheInvalidator cacheInvalidator)
    : IRequestHandler<UpdateCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (result.IsFailure)
            return result;

        var category = result.Value;

        // Unique Name per (Name + ParentId) — exclude self from check
        var spec = new CategoryByNameSpecification(request.Name, category.ParentId, request.Id);
        var nameExists = await categoryRepository.AnyAsync(spec, cancellationToken);

        if (nameExists)
            return Result.Failure(CategoryErrors.DuplicateName(request.Name));

        // DDD: mutation through domain method — UpdatedAt via AuditableEntityInterceptor
        category.Update(request.Name, request.IsActive, request.IconUrl);

        return await context.SaveAndInvalidateAsync(cacheInvalidator, "categories-tag", cancellationToken);
    }
}
