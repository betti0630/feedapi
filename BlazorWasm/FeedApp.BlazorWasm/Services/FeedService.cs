using AttrectoTest.BlazorWasm.Services.Base;

using AutoMapper;

using FeedApp.Blazor.Common.Contracts;
using FeedApp.Blazor.Common.Models;
using FeedApp.BlazorWasm.Configuration;
using FeedApp.BlazorWasm.Services.Base;

using Microsoft.Extensions.Options;

namespace FeedApp.BlazorWasm.Services;

internal class FeedService : BaseHttpService, IFeedService
{
    private readonly IFeedsClient _client;
    private readonly IMapper _mapper;
    private readonly ApiSettings _settings;

    public FeedService(IFeedsClient client, IMapper mapper, IOptions<ApiSettings> settings) 
    {
        ArgumentNullException.ThrowIfNull(settings);

        _client = client;
        _mapper = mapper;
        _settings = settings.Value;
    }       


    public async Task<FeedListModel> GetFeeds()
    {
        var feeds = await _client.GetAsync(1, 100, ListSort.CreatedAtDesc, false);
        var model = new FeedListModel();
        model.Items = _mapper.Map<List<FeedItemModel>>(feeds.Items.ToList());
        foreach (var feed in model.Items)
        {
            if (feed is ImageFeedItemModel imageFeed)
            {
                var baseUri = new Uri(_settings.FeedApiUrl);
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
        catch (ApiException)
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
        catch (ApiException)
        {
            return false;
        }
    }

    public async Task DeleteFeed(int feedId)
    {
        await _client.DeleteAsync(feedId);
    }

    public async Task AddFeed(FeedEditModel feed)
    {
        FileParameter? fileParameter = null;
        if (feed.ImageContent != null)
        {
            var stream = new MemoryStream(feed.ImageContent);

            fileParameter = new FileParameter(stream, feed.ImageFileName, "image/jpeg");
        }
        if (!string.IsNullOrEmpty(feed.VideoUrl))
        {
            await _client.CreateVideoFeedAsync(feed.VideoUrl, feed.Title, feed.Content, fileParameter);

        }
        else if (!string.IsNullOrEmpty(feed.ImageUrl))
        {
            await _client.CreateImageFeedAsync(feed.Title, feed.Content, fileParameter);
        }
        else
        {
            var command = new CreateFeedCommand()
            {
                Title = feed.Title,
                Content = feed.Content
            };
            await _client.PostAsync(command);
        }

    }
}

