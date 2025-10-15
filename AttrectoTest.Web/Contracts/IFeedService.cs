using AttrectoTest.Web.Services.Base;

namespace AttrectoTest.Web.Contracts;

public interface IFeedService
{
    Task<PagedFeeds> GetFeeds();
}
