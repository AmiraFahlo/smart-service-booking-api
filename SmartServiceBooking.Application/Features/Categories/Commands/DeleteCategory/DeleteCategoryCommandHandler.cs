using MediatR;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Entities.Service.Specifications;
using SmartServiceBooking.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace SmartServiceBooking.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IServiceRepository serviceRepository,
    IApplicationDbContext context,
    ICacheInvalidator cacheInvalidator)
    : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryResult = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (categoryResult.IsFailure)
            return Result.Failure(categoryResult.Error);

        var category = categoryResult.Value;

        // ─── SOFT DELETE PROTECTION ──────────────────────────────────────────
        // Business Rule: Cannot delete a category that owns active services.
        // Prevents orphaned services losing their category reference.
        // ─────────────────────────────────────────────────────────────────────
        var spec = new ActiveServicesByCategorySpecification(request.Id);
        var serviceCount = await serviceRepository.CountAsync(spec, cancellationToken);

        if (serviceCount > 0)
            return Result.Failure(CategoryErrors.HasActiveServices(category.Name, serviceCount));

        // DDD: domain method handles state transition
        category.SoftDelete();

        await context.SaveChangesAsync(cancellationToken);

        await cacheInvalidator.InvalidateByTagAsync("categories-tag", cancellationToken);

        return Result.Success();
    }
}
