namespace AttrectoTest.ApiService.Dtos.Feeds;

public class VideoFeedUpdateDto: ImageFeedUpdateDto
{
    public string VideoUrl { get; set; } = default!;
}
