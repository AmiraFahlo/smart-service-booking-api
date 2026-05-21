using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Domain.Shared;
using System.Linq.Expressions;

namespace SmartServiceBooking.Application.Common.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    /// Attempts to find the first element of a sequence that satisfies a specified condition, 
    /// or returns a Failure Result with the provided error if no such element is found.
    /// </summary>
    public static async Task<Result<T>> FirstOrErrorAsync<T>(
        this IQueryable<T> query,
        Expression<Func<T, bool>> predicate,
        Error error,
        CancellationToken cancellationToken = default)
    {
        var entity = await query.FirstOrDefaultAsync(predicate, cancellationToken);

        return entity is not null 
            ? Result.Success(entity) 
            : Result.Failure<T>(error);
    }

    /// <summary>
    /// Attempts to find an entity by its Id, or returns a Failure Result if not found.
    /// Assumes the entity has an 'Id' property (common in this project).
    /// </summary>
    public static async Task<Result<T>> GetByIdOrErrorAsync<T, TId>(
        this IQueryable<T> query,
        TId id,
        Error error,
        CancellationToken cancellationToken = default)
        where T : class
    {
        // This is a simplified version. For a more robust one, we'd use reflection or a base interface.
        // Given the project uses 'int' or 'long' for Ids, we can use a dynamic approach or an interface.
        // Assuming entities might implement a base interface like IEntity<TId>.
        
        // Let's stick to the predicate version for now as it's safer without knowing the exact base interfaces.
        return await query.FirstOrErrorAsync(e => EF.Property<TId>(e!, "Id")!.Equals(id), error, cancellationToken);
    }

    /// <summary>
    /// Converts an IQueryable to a PagedList by performing both the count and the paginated data fetch.
    /// </summary>
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return PagedList<T>.Create(items, count, pageNumber, pageSize);
    }
}
