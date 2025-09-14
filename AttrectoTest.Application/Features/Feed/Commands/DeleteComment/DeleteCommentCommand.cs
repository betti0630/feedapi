using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteComment;

public class DeleteCommentCommand : IRequest
{
    public int FeedId { get; set; }
    public int CommentId { get; set; }
}
