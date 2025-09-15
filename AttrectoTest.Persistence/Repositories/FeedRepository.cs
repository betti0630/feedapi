using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;
using AttrectoTest.Persistence.DatabaseContext;


namespace AttrectoTest.Persistence.Repositories;

internal class FeedRepository(TestDbContext dbContext) : GenericRepository<Feed>(dbContext), IFeedRepository
{
}
