using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteFeed;

#pragma warning disable CA1812
internal sealed class DeleteFeedCommandHandler(IFeedRepository feedRepository, IAppLogger<DeleteFeedCommandHandler> logger) : IRequestHandler<DeleteFeedCommand>
{
    public async Task Handle(DeleteFeedCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);

        if (validationResult.Errors.Count != 0) { 
            throw new BadRequestException("Invalid feed", validationResult);
        }

        var feed = await feedRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false) ?? throw new NotFoundException(nameof(Domain.Feed), request.Id);
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot delete a deleted feed.");
        }
        if (feed.AuthorId != request.UserId)
        {
            logger.LogWarning("User {AuthorId} attempted to delete feed {FeedId} without permission.", request.UserId, feed.Id);
            throw new BadRequestException("User does not have permission to delete this feed.");
        }

        feed.IsDeleted = true;
        await feedRepository.UpdateAsync(feed, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Feed {FeedId} deleted successfully by user {AuthorId}.", feed.Id, request.UserId);
    }
}
