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

    public async Task<int> GetLikesCountAsync(int feedId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<FeedLike>().CountAsync(fl => fl.FeedId == feedId, cancellationToken);
    }

    public async Task AddLikeAsync(int feedId, int userId, CancellationToken cancellationToken = default)
    {
        if (await _dbContext.Set<FeedLike>().AnyAsync(fl => fl.FeedId == feedId && fl.UserId == userId, cancellationToken))
        {
            return;
        }
        await _dbContext.Set<FeedLike>().AddAsync(new FeedLike
        {
            FeedId = feedId,
            UserId = userId
        }, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveLikeAsync(int feedId, int userId, CancellationToken cancellationToken = default)
    {
        if (!await _dbContext.Set<FeedLike>().AnyAsync(fl => fl.FeedId == feedId && fl.UserId == userId, cancellationToken))
        {
            return;
        }
        _dbContext.Set<FeedLike>().Remove(new FeedLike
        {
            FeedId = feedId,
            UserId = userId
        });
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
