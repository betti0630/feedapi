using AttrectoTest.Domain;

namespace AttrectoTest.Application.Contracts.Persistence;

public interface IFeedRepository: IGenericRepository<Feed>
{
    Task CreateImageFeedAsync(ImageFeed entity);
    Task CreateVideoFeedAsync(VideoFeed entity);
    Task<ImageFeed?> GetImageFeedByIdAsync(int id);
    Task<VideoFeed?> GetVideoFeedByIdAsync(int id);
    Task UpdateImageFeedAsync(ImageFeed entity);
    Task UpdateVideoFeedAsync(VideoFeed entity);
}
