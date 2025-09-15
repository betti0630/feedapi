using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;
using AttrectoTest.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;


namespace AttrectoTest.Persistence.Repositories;

internal class FeedRepository(TestDbContext dbContext) : GenericRepository<Feed>(dbContext), IFeedRepository
{
    public override IQueryable<Feed> List()
    {
        return _dbContext.Set<Feed>().Include(f => f.Author).AsNoTracking().AsQueryable();
    }

    public override async Task<Feed?> GetByAsync(Expression<Func<Feed, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Feed>().Include(f => f.Author).AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }
}
