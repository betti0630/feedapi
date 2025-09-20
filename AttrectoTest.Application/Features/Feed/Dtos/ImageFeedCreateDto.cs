using Microsoft.AspNetCore.Http;

namespace AttrectoTest.Application.Features.Feed.Dtos;

public class ImageFeedCreateDto
{
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public IFormFile File { get; set; } = default!;
}
