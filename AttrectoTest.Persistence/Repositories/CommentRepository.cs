using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;
using AttrectoTest.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;

namespace AttrectoTest.Persistence.Repositories;

internal class CommentRepository(IDbContextFactory<TestDbContext> contextFactory) : GenericRepository<Comment>(contextFactory), ICommentRepository
{
   
}
