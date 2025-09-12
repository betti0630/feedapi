using AttrectoTest.Domain.Common;

namespace AttrectoTest.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : AppEntity
{
    Task<IReadOnlyList<T>> GetAsync();
    Task<T?> GetByIdAsync(int id);
    Task<bool> AnyAsync();
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
