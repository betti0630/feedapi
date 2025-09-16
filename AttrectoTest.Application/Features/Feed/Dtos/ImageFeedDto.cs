
namespace AttrectoTest.Application.Features.Feed.Dtos;

public record ImageFeedDto : FeedDto
{
    public string ImageUrl { get; set; } = null!;
}
