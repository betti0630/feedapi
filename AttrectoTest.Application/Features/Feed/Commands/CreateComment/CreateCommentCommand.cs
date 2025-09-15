using AttrectoTest.Application.Features.Base;
using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.CreateComment;

public record CreateCommentCommand : UserRequest, IRequest<CommentDto>
{
    public required int FeedId { get; set; }
    public string Text { get; set; } = null!;
}
