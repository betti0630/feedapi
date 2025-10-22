using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Commands.AddLike;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteLike;

internal sealed class DeleteLikeCommandHandler(IFeedRepository feedRepository, IAppLogger<AddLikeCommandHandler> logger) : IRequestHandler<DeleteLikeCommand>
{
    public async Task Handle(DeleteLikeCommand request, CancellationToken cancellationToken)
    {

        var feed = await feedRepository.GetByIdAsync(request.FeedId, cancellationToken).ConfigureAwait(false) ?? throw new NotFoundException(nameof(Domain.Feed), request.FeedId);
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot update a deleted feed.");
        }
        await feedRepository.RemoveLikeAsync(request.FeedId, request.UserId, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Feed {FeedId} like removed successfully by user {AuthorId}.", feed.Id, request.UserId);
    }
}
