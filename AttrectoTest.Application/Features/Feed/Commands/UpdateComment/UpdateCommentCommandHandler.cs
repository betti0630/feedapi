using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateComment;

internal sealed class UpdateCommentCommandHandler(IFeedRepository feedRepository, ICommentRepository commentRepository) : IRequestHandler<UpdateCommentCommand, CommentDto>
{
    public async Task<CommentDto> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var feed = await feedRepository.GetByIdAsync(request.FeedId, cancellationToken).ConfigureAwait(false) ?? throw new KeyNotFoundException($"Feed with id {request.FeedId} not found");
        if (feed.IsDeleted)
        {
            throw new KeyNotFoundException($"Feed with id {request.FeedId} not found");
        }
        var comment = await commentRepository.GetByIdAsync(request.CommentId, cancellationToken).ConfigureAwait(false) ?? throw new KeyNotFoundException($"Comment with id {request.CommentId} not found.");
        if (comment.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this comment.");
        }
        if (request.Content is not null)
        {

            if (request.Content.Length > 500)
            {
                throw new ArgumentException("Comment content cannot exceed 500 characters.");
            }
            comment.Content = request.Content;
            await commentRepository.UpdateAsync(comment, cancellationToken).ConfigureAwait(false);
        }
        return new CommentDto(feed.Id, comment.Id, comment.Content, comment.DateCreated, comment.DateModified, comment.UserId, request.UserId);
    }
}
