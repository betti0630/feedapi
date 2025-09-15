using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Domain;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;

internal class UpdateFeedCommandHandler : 
    IRequestHandler<UpdateFeedCommand, UpdateFeedCommandResponse>,
    IRequestHandler<UpdateImageFeedCommand, UpdateFeedCommandResponse>,
    IRequestHandler<UpdateVideoFeedCommand, UpdateFeedCommandResponse>
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

    public async Task<UpdateFeedCommandResponse> Handle(UpdateFeedCommand request, CancellationToken cancellationToken)
    {
        var feed =await ValidateRequestAndGetUpdatedFeed<Domain.Feed>(request);

        await _feedRepository.UpdateAsync(feed);
        _logger.LogInformation("Feed {FeedId} updated successfully by user {UserId}.", feed.Id, _authService.UserId ?? -1);
        return MapToResponse(feed);
    }

    public async Task<UpdateFeedCommandResponse> Handle(UpdateImageFeedCommand request, CancellationToken cancellationToken)
    {
        var feed = await ValidateRequestAndGetUpdatedFeed<ImageFeed>(request);

        if (feed.ImageData is not null) { 
            feed.ImageData = request.ImageData;
        }
        await _feedRepository.UpdateAsync(feed);
        _logger.LogInformation("Image feed {FeedId} updated successfully by user {UserId}.", feed.Id, _authService.UserId ?? -1);
        return MapToResponse(feed);
    }

    public async Task<UpdateFeedCommandResponse> Handle(UpdateVideoFeedCommand request, CancellationToken cancellationToken)
    {
        var feed = await ValidateRequestAndGetUpdatedFeed<VideoFeed>(request);

        if (request.ImageData is not null) { 
            feed.ImageData = request.ImageData;
        }
        if (request.VideoUrl is not null) { 
            feed.VideoUrl = request.VideoUrl;
        }
        await _feedRepository.UpdateAsync(feed);
        _logger.LogInformation("Video feed {FeedId} updated successfully by user {UserId}.", feed.Id, _authService.UserId ?? -1);
        return MapToResponse(feed);
    }

    private async Task<TFeed> ValidateRequestAndGetUpdatedFeed<TFeed>(UpdateFeedCommand request) where TFeed : Domain.Feed
    {
        var validator = new UpdateFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any())
        {
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
        if (feed is not TFeed)
        {
            throw new BadRequestException($"Feed is not of type {typeof(TFeed).Name}.");
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
        if (request.Title is not null) { 
            feed.Title = request.Title;
        }
        if (request.Content is not null) { 
            feed.Content = request.Content;
        }
        return (TFeed)feed;
    }

    private UpdateFeedCommandResponse MapToResponse(Domain.Feed feed)
    {
        return new UpdateFeedCommandResponse
        {
            Id = feed.Id,
            Title = feed.Title,
            PublishedAt = feed.PublishedAt
        };
    }
}
