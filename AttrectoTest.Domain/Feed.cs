using AttrectoTest.Domain.Common;

using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.Domain;

public class Feed : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = null!;
}
