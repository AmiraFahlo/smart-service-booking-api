using MediatR;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace SmartServiceBooking.Application.Features.Categories.Commands.RestoreCategory;

public class RestoreCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IApplicationDbContext context,
    ICacheInvalidator cacheInvalidator)
    : IRequestHandler<RestoreCategoryCommand, Result>
{
    public async Task<Result> Handle(RestoreCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await categoryRepository.GetDeletedByIdAsync(request.Id, cancellationToken);

        if (result.IsFailure)
            return Result.Failure(CategoryErrors.NotFoundDeleted(request.Id));

        var category = result.Value;

        // DDD: domain method handles state restoration
        category.Restore();

        await context.SaveChangesAsync(cancellationToken);

        await cacheInvalidator.InvalidateByTagAsync("categories-tag", cancellationToken);

        return Result.Success();
    }
}
