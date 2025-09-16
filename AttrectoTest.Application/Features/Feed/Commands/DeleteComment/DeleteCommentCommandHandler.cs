using AttrectoTest.Application.Contracts.Persistence;


using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteComment;

internal class DeleteCommentCommandHandler(IFeedRepository feedRepository, ICommentRepository commentRepository) : IRequestHandler<DeleteCommentCommand>
{
    public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var feed = await feedRepository.GetByIdAsync(request.FeedId, cancellationToken) ?? throw new KeyNotFoundException($"Feed with id {request.FeedId} not found");
        if (feed.IsDeleted)
        {
            throw new KeyNotFoundException($"Feed with id {request.FeedId} not found");
        }
        var comment = await commentRepository.GetByIdAsync(request.CommentId) ?? throw new KeyNotFoundException($"Comment with id {request.CommentId} not found.");

        //feed owner or comment owner can delete the comment
        if (comment.UserId != request.UserId && feed.AuthorId != request.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this comment.");
        }

        await commentRepository.DeleteAsync(comment, cancellationToken);
    }
}
