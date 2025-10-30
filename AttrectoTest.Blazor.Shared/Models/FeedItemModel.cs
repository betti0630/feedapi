namespace AttrectoTest.Blazor.Common.Models;

public class FeedItemModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? AuthorUserName { get; set; }

    public int AuthorId { get; set; }

    public bool IsOwnFeed { get; set; }

    public DateTimeOffset PublishedAt { get; set; } = DateTime.UtcNow;

    public int LikeCount { get; set; }

    public bool IsLiked { get; set; }

}

public class RssFeedItemModel : FeedItemModel
{
    public string Link { get; set; } = null!;
}

public class ImageFeedItemModel : FeedItemModel
{
    public string ImageUrl { get; set; } = null!;
}

public class VideoFeedItemModel : ImageFeedItemModel
{
    public string VideoUrl { get; set; } = null!;
}