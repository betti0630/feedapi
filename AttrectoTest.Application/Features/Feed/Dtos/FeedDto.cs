
using System.Text.Json.Serialization;

namespace AttrectoTest.Application.Features.Feed.Dtos;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(FeedDto), "text")]
[JsonDerivedType(typeof(ImageFeedDto), "image")]
[JsonDerivedType(typeof(VideoFeedDto), "video")]
public record FeedDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string AuthorUserName { get; set; } = null!;

    public int AuthorId { get; set; }

    public bool IsOwnFeed { get; set; }

    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

    public int LikeCount { get; }

    public bool IsDeleted { get; }
}
