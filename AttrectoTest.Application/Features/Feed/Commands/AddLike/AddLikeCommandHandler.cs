using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.AddLike;

internal class AddLikeCommandHandler(IFeedRepository feedRepository, IAppLogger<AddLikeCommandHandler> logger) : IRequestHandler<AddLikeCommand>
{
    public async Task Handle(AddLikeCommand request, CancellationToken cancellationToken)
    {

        var feed = await feedRepository.GetByIdAsync(request.FeedId, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Feed), request.FeedId);
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot update a deleted feed.");
        }
        await feedRepository.AddLikeAsync(request.FeedId, request.UserId, cancellationToken);
        logger.LogInformation("Feed {FeedId} liked successfully by user {AuthorId}.", feed.Id, request.UserId);
    }
}
