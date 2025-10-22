namespace AttrectoTest.Application.Features.Feed.Dtos;

public class VideoFeedCreateDto: ImageFeedCreateDto
{
    public System.Uri VideoUrl { get; set; } = default!;
}
