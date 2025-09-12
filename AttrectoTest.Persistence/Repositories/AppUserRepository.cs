using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;
using AttrectoTest.Persistence.DatabaseContext;

namespace AttrectoTest.Persistence.Repositories;

internal class AppUserRepository : GenericRepository<AppUser>, IGenericRepository<AppUser>
{

    public AppUserRepository(TestDbContext dbContext) : base(dbContext)
    {
    }

}
