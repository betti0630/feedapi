using AttrectoTest.Domain;

using Microsoft.EntityFrameworkCore;

namespace AttrectoTest.Persistence.DatabaseContext;

public class TestDbContext : DbContext
{
	public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {

	}

    public DbSet<Feed> Feeds => Set<Feed>();
}
