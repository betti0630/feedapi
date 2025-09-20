namespace AttrectoTest.Application.Features.Feed.Dtos;

public class VideoFeedCreateDto: ImageFeedCreateDto
{
    public string VideoUrl { get; set; } = default!;
}
