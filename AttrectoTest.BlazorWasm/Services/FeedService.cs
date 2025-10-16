using AttrectoTest.Blazor.Shared.Contracts;
using AttrectoTest.Blazor.Shared.Models;
using AttrectoTest.BlazorWasm.Configuration;
using AttrectoTest.BlazorWasm.Services.Base;

using AutoMapper;

using Microsoft.Extensions.Options;

namespace AttrectoTest.BlazorWasm.Services;

public class FeedService : BaseHttpService, IFeedService
{
    protected readonly IFeedsClient _client;
    private readonly IMapper _mapper;
    private readonly ApiSettings _settings;

    public FeedService(IFeedsClient client, IMapper mapper, IOptions<ApiSettings> settings) 
    {
        _client = client;
        _mapper = mapper;
        _settings = settings.Value;
    }       


    public async Task<FeedListModel> GetFeeds()
    {
        var feeds = await _client.GetAsync(1, 100, ListSort.CreatedAt_desc, false);
        var model = new FeedListModel();
        model.Items = _mapper.Map<List<FeedItemModel>>(feeds.Items.ToList());
        foreach (var feed in model.Items)
        {
            if (feed is ImageFeedItemModel imageFeed)
            {
                var baseUri = new Uri(_settings.BaseUrl);
                imageFeed.ImageUrl = new Uri(baseUri, imageFeed.ImageUrl).ToString();
            }
        }
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
