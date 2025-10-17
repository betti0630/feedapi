using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;
using AttrectoTest.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;

namespace AttrectoTest.Persistence.Repositories;

internal class AppUserRepository : GenericRepository<AppUser>, IGenericRepository<AppUser>
{

    public AppUserRepository(IDbContextFactory<TestDbContext> contextFactory) : base(contextFactory)
    {
    }

}
