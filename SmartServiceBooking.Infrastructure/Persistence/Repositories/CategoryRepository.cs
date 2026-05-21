using SmartServiceBooking.Domain.Entities.Service;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Infrastructure.Persistence.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public async Task<Result<Category>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return category is not null 
            ? Result.Success(category) 
            : Result.Failure<Category>(new Error("Category.NotFound", $"Category with Id {id} was not found."));
    }

    public async Task<Result<Category>> GetDeletedByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await context.Categories
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted, cancellationToken);

        return category is not null 
            ? Result.Success(category) 
            : Result.Failure<Category>(new Error("Category.NotFound", $"Deleted Category with Id {id} was not found."));
    }

    public async Task<bool> AnyAsync(ISpecification<Category> spec, CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator<Category>
            .GetQuery(context.Categories, spec)
            .AnyAsync(cancellationToken);
    }

    public void Add(Category category)
    {
        context.Categories.Add(category);
    }

    public void Update(Category category)
    {
        context.Categories.Update(category);
    }
}
