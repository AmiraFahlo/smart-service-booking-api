using MediatR;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Entities.Service.Specifications;
using SmartServiceBooking.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

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

        await context.SaveChangesAsync(cancellationToken);

        await cacheInvalidator.InvalidateByTagAsync("categories-tag", cancellationToken);

        return Result.Success();
    }
}
