using AttrectoTest.Blazor.Shared.Models;

namespace AttrectoTest.Blazor.Shared.Contracts;

public interface IFeedService
{
    Task<FeedListModel> GetFeeds();
}
