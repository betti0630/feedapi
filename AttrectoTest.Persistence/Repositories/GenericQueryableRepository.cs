using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain.Common;
using AttrectoTest.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace AttrectoTest.Persistence.Repositories;

internal class GenericQueryableRepository<T> :
    GenericRepository<T>, IQueryableRepository<T> where T : AppEntity
{
    private readonly List<TestDbContext> _dbContextList = new();

    public GenericQueryableRepository(IDbContextFactory<TestDbContext> contextFactory) : base(contextFactory)
    {
    }

    public virtual IQueryable<T> List()
    {
        var dbContext = _contextFactory.CreateDbContext();
        return dbContext.Set<T>().AsNoTracking().AsQueryable();
    }



    protected TestDbContext CreateNewContext()
    {
        var retVal = _contextFactory.CreateDbContext();
        if (_dbContextList.Count > 25)
        {
            _dbContextList.RemoveAt(0);
        }
        _dbContextList.Add(retVal);
        return retVal;
    }

    public void Dispose()
    {
        foreach(var context in _dbContextList)
        {
            context.Dispose();
        }
    }
}
