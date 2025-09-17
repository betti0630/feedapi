using AttrectoTest.Domain.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttrectoTest.Domain;

public class Feed : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(AuthorId))]
    public AppUser Author { get; set; } = null!;

    [Required]
    public int AuthorId { get; set; }

    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;

    public ICollection<FeedLike> Likes { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];

}
