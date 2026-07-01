namespace SmartServiceBooking.Application.Common.Interfaces;

public interface ICategoryRepository
{
    Task<Result<Category>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    
    Task<Result<Category>> GetDeletedByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(ISpecification<Category> spec, CancellationToken cancellationToken = default);
    
    void Add(Category category);
    
    void Update(Category category);
}
