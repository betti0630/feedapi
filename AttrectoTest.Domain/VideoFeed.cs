using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.Domain;

public class VideoFeed : ImageFeed
{
    [Required]
    [MaxLength(1000)]
    public string VideoUrl { get; set; } = null!;
}
