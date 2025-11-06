using FeedApp.Blazor.Common.Models;

namespace FeedApp.Blazor.Common.Contracts;

public interface IFeedService
{
    Task<FeedListModel> GetFeeds();
    Task AddFeed(FeedEditModel feed);
    Task DeleteFeed(int feedId);
    Task<bool> AddLike(int feedId);
    Task<bool> DeleteLike(int feedId);
}
