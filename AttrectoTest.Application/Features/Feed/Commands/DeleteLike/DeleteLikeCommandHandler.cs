using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Commands.AddLike;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteLike;

internal class DeleteLikeCommandHandler(IFeedRepository feedRepository, IAppLogger<AddLikeCommandHandler> logger) : IRequestHandler<DeleteLikeCommand>
{
    public async Task Handle(DeleteLikeCommand request, CancellationToken cancellationToken)
    {

        var feed = await feedRepository.GetByIdAsync(request.FeedId, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Feed), request.FeedId);
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot update a deleted feed.");
        }
        logger.LogInformation("Feed {FeedId} liked successfully by user {UserId}.", feed.Id, request.UserId);
    }
}
