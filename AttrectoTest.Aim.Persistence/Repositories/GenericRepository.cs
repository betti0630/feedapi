﻿using AttrectoTest.Aim.Persistence.DatabaseContext;
using AttrectoTest.Aim.Application.Contracts.Persistence;
using AttrectoTest.Aim.Domain.Common;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace AttrectoTest.Aim.Persistence.Repositories;

internal class GenericRepository<T>(IDbContextFactory<TestDbContext> contextFactory) : IGenericRepository<T> where T : AppEntity
{
    protected readonly IDbContextFactory<TestDbContext> _contextFactory = contextFactory;

    public virtual async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _contextFactory.CreateDbContext();
        await dbContext.Set<T>().AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _contextFactory.CreateDbContext();
        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = _contextFactory.CreateDbContext();
        return await dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
    }

     public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)     {
        await using var dbContext = _contextFactory.CreateDbContext();
        return await dbContext.Set<T>().AnyAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _contextFactory.CreateDbContext();
        return await dbContext.Set<T>().AnyAsync(predicate, cancellationToken: cancellationToken);
    }

    public virtual async Task<T?> GetByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _contextFactory.CreateDbContext();
        return await dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetByAsync(x => x.Id == id, cancellationToken);
    }


    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _contextFactory.CreateDbContext();
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
