using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteComment;

internal class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
{
    public Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        //feed owner or comment owner can delete the comment
        throw new NotImplementedException();
    }
}
