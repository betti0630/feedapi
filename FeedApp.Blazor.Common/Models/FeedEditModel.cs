using System.ComponentModel.DataAnnotations;

namespace FeedApp.Blazor.Common.Models;

public class FeedEditModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public byte[]? ImageContent { get; set; }

    public string? ImageFileName { get; set; }

    [Url(ErrorMessage = "Not valid URL")]
    public string? VideoUrl { get; set;}
}
