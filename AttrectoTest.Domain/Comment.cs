using AttrectoTest.Domain.Common;

using System.ComponentModel.DataAnnotations.Schema;

namespace AttrectoTest.Domain;

public class Comment : BaseEntity
{
    public string Content { get; set; } = null!;
      
    public int FeedId { get; set; }
    [ForeignKey(nameof(FeedId))]
    public Feed Feed { get; set; } = null!;

    public int UserId { get; set; }
}
