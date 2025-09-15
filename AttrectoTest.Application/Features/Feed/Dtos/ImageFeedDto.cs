
namespace AttrectoTest.Application.Features.Feed.Dtos;

public record ImageFeedDto : FeedDto
{
    public byte[] ImageData { get; set; } = null!;
}
