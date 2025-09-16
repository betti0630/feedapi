using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Domain;
using AttrectoTest.Domain.Common;

using Microsoft.EntityFrameworkCore;

namespace AttrectoTest.Persistence.DatabaseContext;

internal class TestDbContext : DbContext
{
    private readonly IAuthUserService _authService;

    public TestDbContext(DbContextOptions<TestDbContext> options, IAuthUserService authService) : base(options)
    {
        _authService = authService;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
            .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.DateModified = DateTime.Now;
            entry.Entity.ModifiedBy = _authService.UserName;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.Now;
                entry.Entity.CreatedBy = _authService.UserName;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feed>()
            .ToTable("Feeds");
        modelBuilder.Entity<ImageFeed>()
            .ToTable("ImageFeeds");
        modelBuilder.Entity<VideoFeed>()
            .ToTable("VideoFeeds");
        modelBuilder.Entity<FeedLike>()
            .HasKey(fl => new { fl.FeedId, fl.UserId });
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<Feed> Feeds => Set<Feed>();
    public DbSet<FeedLike> FeedLikes => Set<FeedLike>();
    public DbSet<Comment> Comments => Set<Comment>();
}
