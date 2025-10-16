using AttrectoTest.Blazor.Shared.Contracts;
using AttrectoTest.Blazor.Shared.Models;
using AttrectoTest.BlazorWasm.Services.Base;

using AutoMapper;

namespace AttrectoTest.BlazorWasm.Services;

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

    public async Task<bool> AddLike(int feedId)
    {
        try
        {
            await _client.LikePOSTAsync(feedId);
            return true;
        }
        catch (ApiException ex)
        {
            return false;
        }
    }

    public async Task<bool> DeleteLike(int feedId)
    {
        try
        {
            await _client.LikeDELETEAsync(feedId);
            return true;
        }
        catch (ApiException ex)
        {
            return false;
        }
    }

    public async Task DeleteFeed(int feedId)
    {
        await _client.DeleteAsync(feedId);
    }

}
