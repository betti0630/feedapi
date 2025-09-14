using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;

internal class UpdateFeedCommandHandler : IRequestHandler<UpdateFeedCommand, FeedDto>
{
    private readonly IFeedRepository _feedRepository;
    private readonly IAppLogger<UpdateFeedCommandHandler> _logger;
    private readonly IAuthUserService _authService;

    public UpdateFeedCommandHandler(IFeedRepository feedRepository, IAuthUserService authService,
        IAppLogger<UpdateFeedCommandHandler> logger)
    {
        _feedRepository = feedRepository;
        _authService = authService;
        _logger = logger;
    }

    public async Task<FeedDto> Handle(UpdateFeedCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any()) { 
            throw new BadRequestException("Invalid feed", validationResult);
        }
        var userId = _authService.UserId;

        if (userId == null || userId <= 0)
        {
            _logger.LogWarning("Unauthorized attempt to create a feed.");
            throw new BadRequestException("User must be authenticated to create a feed.");
        }

        var feed = await _feedRepository.GetByIdAsync(request.Id);
        if (feed == null)
        {
            throw new NotFoundException(nameof(Domain.Feed), request.Id);
        }
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot update a deleted feed.");
        }
        if (feed.AuthorId != userId)
        {
            _logger.LogWarning("User {UserId} attempted to update feed {FeedId} without permission.", userId, feed.Id);
            throw new BadRequestException("User does not have permission to update this feed.");
        }
        feed.Title = request.Title;
        feed.Content = request.Content;
        await _feedRepository.UpdateAsync(feed);
        _logger.LogInformation("Feed {FeedId} updated successfully by user {UserId}.", feed.Id, userId);
        return new FeedDto()
        {
            Id = feed.Id,
            Title = feed.Title,
            Content = feed.Content,
            AuthorId = feed.AuthorId,
            AuthorUserName = _authService.UserName ?? "Unknown",
            PublishedAt = feed.PublishedAt
        };
    }
}
