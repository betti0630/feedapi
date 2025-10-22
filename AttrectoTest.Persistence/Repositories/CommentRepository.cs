using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;
using AttrectoTest.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;

namespace AttrectoTest.Persistence.Repositories;

internal sealed class CommentRepository(IDbContextFactory<TestDbContext> contextFactory) : GenericQueryableRepository<Comment>(contextFactory), ICommentRepository
{
   
}
