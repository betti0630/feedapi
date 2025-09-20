using Microsoft.AspNetCore.Http;

namespace AttrectoTest.Application.Features.Feed.Dtos;

public class ImageFeedUpdateDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public IFormFile? File { get; set; } 
}
