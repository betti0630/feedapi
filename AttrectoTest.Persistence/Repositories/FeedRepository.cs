using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;
using AttrectoTest.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;

namespace AttrectoTest.Persistence.Repositories;

internal class FeedRepository : GenericRepository<Feed>, IFeedRepository
{
    public FeedRepository(TestDbContext dbContext) : base(dbContext)
    {
    }

    public async Task CreateImageFeedAsync(ImageFeed entity)
    {
        await _dbContext.Set<ImageFeed>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateVideoFeedAsync(VideoFeed entity)
    {
        await _dbContext.Set<VideoFeed>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ImageFeed?> GetImageFeedByIdAsync(int id)
    {
        return await _dbContext.Set<ImageFeed>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<VideoFeed?> GetVideoFeedByIdAsync(int id)
    {
        return await _dbContext.Set<VideoFeed>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateImageFeedAsync(ImageFeed entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateVideoFeedAsync(VideoFeed entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
}
