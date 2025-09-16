
namespace AttrectoTest.Application.Features.Feed.Dtos;

public record class CommentDto
{
    public CommentDto( int @feedId, int @id, string @content, DateTimeOffset? @createdAt, DateTimeOffset? @updatedAt, int @userId)
    {
        this.Id = @id;
        this.FeedId = @feedId;
        this.UserId = @userId;
        this.Content = @content;
        this.CreatedAt = @createdAt;
        this.UpdatedAt = @updatedAt;
    }

    public int Id { get; }

    public int FeedId { get; }

    public int UserId { get; }

    public string Content { get; }

    public System.DateTimeOffset? CreatedAt { get; }

    public System.DateTimeOffset? UpdatedAt { get; }


}
