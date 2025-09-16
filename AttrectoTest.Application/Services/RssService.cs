using AttrectoTest.Application.Features.Feed.Dtos;

using System.ServiceModel.Syndication;
using System.Xml;

namespace AttrectoTest.Application.Services;

public class RssService
{
    private readonly HttpClient _httpClient;

    public RssService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RssFeedDto>> GetLoveMeowFeedAsync(CancellationToken cancellationToken)
    {
        var feedUrl = "https://www.lovemeow.com/feeds/feed.rss";

        using var stream = await _httpClient.GetStreamAsync(feedUrl, cancellationToken);
        using var xmlReader = XmlReader.Create(stream);

        var feed = SyndicationFeed.Load(xmlReader);

        return feed.Items.Select(item => new RssFeedDto
        {
            Title = item.Title.Text,
            Link = item.Links.FirstOrDefault()?.Uri.ToString() ?? "",
            Content = item.Summary?.Text ?? "",
            PublishedAt = item.PublishDate.DateTime
        }).ToList();
    }
}
