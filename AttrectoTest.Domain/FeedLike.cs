using System.ComponentModel.DataAnnotations.Schema;

namespace AttrectoTest.Domain;

public class FeedLike
{
    [ForeignKey(nameof(FeedId))]
    public Feed Feed { get; set; } = null!;
    public int FeedId { get; set; }

    public int UserId { get; set; }
}
