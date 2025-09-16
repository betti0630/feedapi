namespace AttrectoTest.Application.Features.Feed.Dtos;

public record RssFeedDto : FeedDto
{
    public string Link { get; set; } = null!;
}
