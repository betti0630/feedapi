using AttrectoTest.Iam.Application.Contracts.Persistence;
using AttrectoTest.Iam.Domain;
using AttrectoTest.Iam.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;

namespace AttrectoTest.Iam.Persistence.Repositories;

internal sealed class AppUserRepository : GenericRepository<AppUser>, IGenericRepository<AppUser>
{

    public AppUserRepository(IDbContextFactory<AuthDbContext> contextFactory) : base(contextFactory)
    {
    }

}
