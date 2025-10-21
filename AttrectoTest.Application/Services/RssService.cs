using AttrectoTest.Application.Features.Feed.Dtos;

using Microsoft.Extensions.Caching.Memory;

using System.ServiceModel.Syndication;
using System.Xml;

namespace AttrectoTest.Application.Services;

public class RssService(HttpClient httpClient, IMemoryCache cache)
{
    public async Task<List<RssFeedDto>> GetLoveMeowFeedAsync(CancellationToken cancellationToken)
    {
        const string cacheKey = "LoveMeowFeed";

        if (cache.TryGetValue(cacheKey, out List<RssFeedDto>? cached))
        {
            return cached!;
        }

        var feedUrl = "https://www.lovemeow.com/feeds/feed.rss";

        using var stream = await httpClient.GetStreamAsync(feedUrl, cancellationToken).ConfigureAwait(false);
        using var xmlReader = XmlReader.Create(stream);

        var feed = SyndicationFeed.Load(xmlReader);

        var items = feed.Items.Select(item => new RssFeedDto
        {
            Title = item.Title.Text,
            Link = item.Links.FirstOrDefault()?.Uri.ToString() ?? "",
            Content = item.Summary?.Text ?? "",
            PublishedAt = item.PublishDate.DateTime
        }).ToList();

        cache.Set(cacheKey, items, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
        });

        return items;
    }
}
