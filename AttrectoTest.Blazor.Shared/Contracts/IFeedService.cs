using AttrectoTest.Blazor.Common.Models;

namespace AttrectoTest.Blazor.Common.Contracts;

public interface IFeedService
{
    Task<FeedListModel> GetFeeds();
    Task AddFeed(FeedEditModel feed);
    Task DeleteFeed(int feedId);
    Task<bool> AddLike(int feedId);
    Task<bool> DeleteLike(int feedId);
}
