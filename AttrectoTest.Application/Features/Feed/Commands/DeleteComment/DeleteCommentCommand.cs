using AttrectoTest.Application.Features.Base;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteComment;

public record DeleteCommentCommand : UserRequest, IRequest
{
    public int FeedId { get; set; }
    public int CommentId { get; set; }
}
