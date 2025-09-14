using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateComment;

public class UpdateCommentCommand : IRequest<CommentDto>
{
    public int CommentId { get; set; }
    public string Text { get; set; } = null!;
}
