using AttrectoTest.Aim.Application.Contracts.Persistence;
using AttrectoTest.Aim.Domain;
using AttrectoTest.Aim.Persistence.DatabaseContext;


using Microsoft.EntityFrameworkCore;

namespace AttrectoTest.Aim.Persistence.Repositories;

internal class AppUserRepository : GenericRepository<AppUser>, IGenericRepository<AppUser>
{

    public AppUserRepository(IDbContextFactory<TestDbContext> contextFactory) : base(contextFactory)
    {
    }

}
