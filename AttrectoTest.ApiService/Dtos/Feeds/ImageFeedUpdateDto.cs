namespace AttrectoTest.ApiService.Dtos.Feeds;

public class ImageFeedUpdateDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public IFormFile File { get; set; } = default!;
}
