using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;
using AttrectoTest.Persistence.DatabaseContext;

namespace AttrectoTest.Persistence.Repositories;

internal class CommentRepository(TestDbContext dbContext) : GenericRepository<Comment>(dbContext), ICommentRepository
{
   
}
