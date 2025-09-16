
namespace AttrectoTest.Application.Features.Feed.Dtos;

public record class CommentDto
{
    public CommentDto( int @feedId, int @id, string @content, DateTimeOffset? @createdAt, DateTimeOffset? @updatedAt, int authorUserId, int currentUserId)
    {
        Id = @id;
        FeedId = @feedId;
        AuthorId = authorUserId;
        Content = @content;
        CreatedAt = @createdAt;
        UpdatedAt = @updatedAt;
        IsOwnedByCurrentUser = authorUserId == currentUserId;
    }

    public int Id { get; }

    public int FeedId { get; }

    public int AuthorId { get; }

    public string Content { get; }

    public DateTimeOffset? CreatedAt { get; }

    public DateTimeOffset? UpdatedAt { get; }

    public bool IsOwnedByCurrentUser { get; set; }

    }
