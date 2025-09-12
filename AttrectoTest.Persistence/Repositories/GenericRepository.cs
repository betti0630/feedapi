using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain.Common;
using AttrectoTest.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace AttrectoTest.Persistence.Repositories;

internal class GenericRepository<T> : IGenericRepository<T> where T : AppEntity
{
    protected readonly TestDbContext _dbContext;

    public GenericRepository(TestDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync()
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public IQueryable<T> List()
    {
        return _dbContext.Set<T>().AsNoTracking().AsQueryable();
    }


    public Task<bool> AnyAsync()     {
        return _dbContext.Set<T>().AnyAsync();
    }

    public async Task<T?> GetByAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await GetByAsync(x => x.Id == id);
    }


    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
}
