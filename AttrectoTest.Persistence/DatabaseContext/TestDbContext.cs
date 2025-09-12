using AttrectoTest.Domain;

using Microsoft.EntityFrameworkCore;

namespace AttrectoTest.Persistence.DatabaseContext;

internal class TestDbContext : DbContext
{
	public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {

	}

    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<Feed> Feeds => Set<Feed>();
}
