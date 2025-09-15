namespace AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;

public record UpdateFeedCommandResponse()
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
}