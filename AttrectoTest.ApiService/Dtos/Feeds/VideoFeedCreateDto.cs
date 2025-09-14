namespace AttrectoTest.ApiService.Dtos.Feeds;

public class VideoFeedCreateDto: ImageFeedCreateDto
{
    public string VideoUrl { get; set; } = default!;
}
