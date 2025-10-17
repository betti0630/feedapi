using AttrectoTest.Blazor.Shared.Contracts;
using AttrectoTest.Blazor.Shared.Models;

namespace AttrecotTest.BlazorServer.Services;

public class FeedService : IFeedService
{
    public Task<bool> AddLike(int feedId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFeed(int feedId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteLike(int feedId)
    {
        throw new NotImplementedException();
    }

    public Task<FeedListModel> GetFeeds()
    {
        throw new NotImplementedException();
    }
}
