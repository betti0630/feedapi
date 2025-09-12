using AttrectoTest.Domain.Common;

using System.Linq.Expressions;

namespace AttrectoTest.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : AppEntity
{
    Task<IReadOnlyList<T>> GetAsync();
    Task<T?> GetByAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetByIdAsync(int id);
    IQueryable<T> List();
    Task<bool> AnyAsync();
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
