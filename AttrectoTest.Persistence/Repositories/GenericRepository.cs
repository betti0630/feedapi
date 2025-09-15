using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain.Common;
using AttrectoTest.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace AttrectoTest.Persistence.Repositories;

internal class GenericRepository<T>(TestDbContext dbContext) : IGenericRepository<T> where T : AppEntity
{
    protected readonly TestDbContext _dbContext = dbContext;

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
    }

    public IQueryable<T> List()
    {
        return _dbContext.Set<T>().AsNoTracking().AsQueryable();
    }


    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)     {
        return _dbContext.Set<T>().AnyAsync(cancellationToken: cancellationToken);
    }

    public async Task<T?> GetByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetByAsync(x => x.Id == id, cancellationToken);
    }


    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
