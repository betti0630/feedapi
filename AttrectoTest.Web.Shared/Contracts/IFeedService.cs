using AttrectoTest.Web.Shared.Models;

namespace AttrectoTest.Web.Contracts;

public interface IFeedService
{
    Task<FeedListModel> GetFeeds();
}
