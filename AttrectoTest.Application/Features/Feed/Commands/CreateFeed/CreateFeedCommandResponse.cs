namespace AttrectoTest.Application.Features.Feed.Commands.CreateFeed;

public record CreateFeedCommandResponse()
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
}