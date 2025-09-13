using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;
using AttrectoTest.Persistence.DatabaseContext;

namespace AttrectoTest.Persistence.Repositories;

internal class FeedRepository : GenericRepository<Feed>, IFeedRepository
{
    public FeedRepository(TestDbContext dbContext) : base(dbContext)
    {
    }
}
