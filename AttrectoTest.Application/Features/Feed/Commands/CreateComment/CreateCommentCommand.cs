using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.CreateComment;

public class CreateCommentCommand : IRequest<CommentDto>
{
    public Guid FeedId { get; set; }
    public string Text { get; set; } = null!;
}
