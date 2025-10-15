
using System.Text.Json.Serialization;

namespace AttrectoTest.Application.Features.Feed.Dtos;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(FeedDto), nameof(FeedDto))]
[JsonDerivedType(typeof(ImageFeedDto), nameof(ImageFeedDto))]
[JsonDerivedType(typeof(VideoFeedDto), nameof(VideoFeedDto))]
[JsonDerivedType(typeof(RssFeedDto), nameof(RssFeedDto))]
public record FeedDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? AuthorUserName { get; set; }

    public int AuthorId { get; set; }

    public bool IsOwnFeed { get; set; }

    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

    public int LikeCount { get; set; }

    public bool IsLiked { get; set; }

    public bool IsDeleted { get; }
}
