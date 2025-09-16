using AttrectoTest.Application.Features.Base;
using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateComment;

public record UpdateCommentCommand : UserRequest, IRequest<CommentDto>
{
    public required int FeedId { get; set; }
    public required int CommentId { get; set; }
    public string? Content { get; set; }
}
