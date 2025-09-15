using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Commands.CreateFeed;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteFeed;

internal class DeleteFeedCommandHandler : IRequestHandler<DeleteFeedCommand>
{
    private readonly IFeedRepository _feedRepository;
    private readonly IAppLogger<DeleteFeedCommandHandler> _logger;

    public DeleteFeedCommandHandler(IFeedRepository feedRepository, IAppLogger<DeleteFeedCommandHandler> logger)
    {
        _feedRepository = feedRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteFeedCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0) { 
            throw new BadRequestException("Invalid feed", validationResult);
        }

        var feed = await _feedRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(Domain.Feed), request.Id);
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot delete a deleted feed.");
        }
        if (feed.AuthorId != request.UserId)
        {
            _logger.LogWarning("User {UserId} attempted to delete feed {FeedId} without permission.", request.UserId, feed.Id);
            throw new BadRequestException("User does not have permission to delete this feed.");
        }

        feed.IsDeleted = true;
        await _feedRepository.UpdateAsync(feed);
        _logger.LogInformation("Feed {FeedId} deleted successfully by user {UserId}.", feed.Id, request.UserId);
    }
}
