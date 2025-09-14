
namespace AttrectoTest.Application.Features.Feed.Dtos;

public record ImageFeedDto : FeedDto
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
