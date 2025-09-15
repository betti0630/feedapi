
namespace AttrectoTest.Application.Features.Feed.Dtos;

public record VideoFeedDto : ImageFeedDto
{
    public string VideoUrl { get; set; } = null!;
}
