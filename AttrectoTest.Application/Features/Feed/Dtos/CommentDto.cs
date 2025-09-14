using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.Application.Features.Feed.Dtos;

public record class CommentDto
{
    public CommentDto(System.DateTimeOffset @createdAt, Guid @feedId, Guid @id, string @text, System.DateTimeOffset? @updatedAt, Guid @userId)
    {
        this.Id = @id;
        this.FeedId = @feedId;
        this.UserId = @userId;
        this.Text = @text;
        this.CreatedAt = @createdAt;
        this.UpdatedAt = @updatedAt;
    }

    [Required(AllowEmptyStrings = true)]
    public Guid Id { get; }

    [Required(AllowEmptyStrings = true)]
    public Guid FeedId { get; }

    [Required(AllowEmptyStrings = true)]
    public Guid UserId { get; }

    [Required(AllowEmptyStrings = true)]
    public string Text { get; }

    [Required(AllowEmptyStrings = true)]
    public System.DateTimeOffset CreatedAt { get; }

    public System.DateTimeOffset? UpdatedAt { get; }


}
