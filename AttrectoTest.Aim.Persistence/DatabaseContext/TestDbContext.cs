using AttrectoTest.Aim.Domain;
using AttrectoTest.Aim.Domain.Common;

using Microsoft.EntityFrameworkCore;

namespace AttrectoTest.Aim.Persistence.DatabaseContext;

internal class TestDbContext : DbContext
{
    //private readonly IAuthUserService _authService;

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
        //_authService = authService;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
            .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.DateModified = DateTime.Now;
            //entry.Entity.ModifiedBy = _authService.UserName;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.Now;
                //entry.Entity.CreatedBy = _authService.UserName;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<AppUser> AppUsers => Set<AppUser>(); 
 }
    
