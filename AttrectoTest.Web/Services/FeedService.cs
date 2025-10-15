using AttrectoTest.Web.Contracts;
using AttrectoTest.Web.Services.Base;

namespace AttrectoTest.Web.Services;

public class FeedService : BaseHttpService, IFeedService
{
    protected IFeedsClient _client;

    public FeedService(IFeedsClient client) 
    {
        _client = client;
    }       

    public async Task<PagedFeeds> GetFeeds()
    {
        var feeds = await _client.GetAsync(1, 100, ListSort.CreatedAt_desc, false);
        return feeds;
    }

}
