using AttrectoTest.Domain;
using AttrectoTest.Domain.Common;

using Microsoft.EntityFrameworkCore;

namespace AttrectoTest.Persistence.DatabaseContext;

internal class TestDbContext : DbContext
{

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {

	}

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
            .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.DateModified = DateTime.Now;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.Now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<Feed> Feeds => Set<Feed>();
}
