using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;
using AttrectoTest.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

using System.Linq.Expressions;


namespace AttrectoTest.Persistence.Repositories;

internal sealed class FeedRepository(IDbContextFactory<TestDbContext> contextFactory) : GenericQueryableRepository<Feed>(contextFactory), IFeedRepository, IDisposable
{
    public override IQueryable<Feed> List()
    {
        var dbContext = CreateNewContext();
        return dbContext.Set<Feed>().AsNoTracking().AsQueryable();
    }

    public override async Task<Feed?> GetByAsync(Expression<Func<Feed, bool>> predicate, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _contextFactory.CreateDbContext();
        return await dbContext.Set<Feed>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }

    public async Task<int> GetLikesCountAsync(int feedId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _contextFactory.CreateDbContext();
        return await dbContext.Set<FeedLike>().CountAsync(fl => fl.FeedId == feedId, cancellationToken);
    }

    public async Task AddLikeAsync(int feedId, int userId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _contextFactory.CreateDbContext();
        if (await dbContext.Set<FeedLike>().AnyAsync(fl => fl.FeedId == feedId && fl.UserId == userId, cancellationToken))
        {
            return;
        }
        await dbContext.Set<FeedLike>().AddAsync(new FeedLike
        {
            FeedId = feedId,
            UserId = userId
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveLikeAsync(int feedId, int userId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _contextFactory.CreateDbContext();
        if (!await dbContext.Set<FeedLike>().AnyAsync(fl => fl.FeedId == feedId && fl.UserId == userId, cancellationToken))
        {
            return;
        }
        dbContext.Set<FeedLike>().Remove(new FeedLike
        {
            FeedId = feedId,
            UserId = userId
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
