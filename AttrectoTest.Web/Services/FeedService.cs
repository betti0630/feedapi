using AttrectoTest.Web.Contracts;
using AttrectoTest.Web.Services.Base;
using AttrectoTest.Web.Shared.Models;

using AutoMapper;

namespace AttrectoTest.Web.Services;

public class FeedService : BaseHttpService, IFeedService
{
    protected readonly IFeedsClient _client;
    private readonly IMapper _mapper;

    public FeedService(IFeedsClient client, IMapper mapper) 
    {
        _client = client;
        _mapper = mapper;
    }       

    public async Task<FeedListModel> GetFeeds()
    {
        var feeds = await _client.GetAsync(1, 100, ListSort.CreatedAt_desc, false);
        var model = new FeedListModel();
        model.Items = _mapper.Map<List<FeedItemModel>>(feeds.Items.ToList());
        return model;
    }

}
