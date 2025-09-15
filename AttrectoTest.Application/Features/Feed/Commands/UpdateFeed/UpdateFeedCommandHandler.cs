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

    public UpdateFeedCommandHandler(IFeedRepository feedRepository, IAppLogger<UpdateFeedCommandHandler> logger)
    {
        _feedRepository = feedRepository;
        _logger = logger;
    }

    public async Task<UpdateFeedCommandResponse> Handle(UpdateFeedCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any())
        {
            throw new BadRequestException("Invalid feed", validationResult);
        }

        var feed = await _feedRepository.GetByIdAsync(request.Id);
        if (feed is not Domain.Feed imageFeed)
        {
            throw new BadRequestException($"Feed is not of type {nameof(Domain.Feed)}.");
        }
        ValidateFeed(feed, request);
        FillFeedByRequest(feed, request);

        await _feedRepository.UpdateAsync(feed);
        _logger.LogInformation("Feed {FeedId} updated successfully by user {UserId}.", feed.Id, request.UserId);
        return MapToResponse(feed);
    }

    public async Task<UpdateFeedCommandResponse> Handle(UpdateImageFeedCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any())
        {
            throw new BadRequestException("Invalid feed", validationResult);
        }

        var feed = await _feedRepository.GetByIdAsync(request.Id);
        if (feed is not ImageFeed imageFeed)
        {
            throw new BadRequestException($"Feed is not of type { nameof(ImageFeed) }.");
        }
        ValidateFeed(imageFeed, request);
        FillFeedByRequest(feed, request);

        if (request.ImageData is not null) {
            imageFeed.ImageData = request.ImageData;
        }
        await _feedRepository.UpdateAsync(imageFeed);
        _logger.LogInformation("Image feed {FeedId} updated successfully by user {UserId}.", imageFeed.Id, request.UserId);
        return MapToResponse(feed);
    }

    public async Task<UpdateFeedCommandResponse> Handle(UpdateVideoFeedCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any())
        {
            throw new BadRequestException("Invalid feed", validationResult);
        }

        var feed = await _feedRepository.GetByIdAsync(request.Id);
        if (feed is not VideoFeed videoFeed)
        {
            throw new BadRequestException($"Feed is not of type {nameof(VideoFeed)}.");
        }
        ValidateFeed(videoFeed, request);
        FillFeedByRequest(videoFeed, request);

        if (request.ImageData is not null) {
            videoFeed.ImageData = request.ImageData;
        }
        if (request.VideoUrl is not null) {
            videoFeed.VideoUrl = request.VideoUrl;
        }
        await _feedRepository.UpdateAsync(videoFeed);
        _logger.LogInformation("Video feed {FeedId} updated successfully by user {UserId}.", videoFeed.Id, request.UserId);
        return MapToResponse(feed);
    }

    private void ValidateFeed(Domain.Feed? feed, UpdateFeedCommand request) 
    {
        if (feed == null)
        {
            throw new NotFoundException(nameof(Domain.Feed), request.Id);
        }
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot update a deleted feed.");
        }
        if (feed.AuthorId != request.UserId)
        {
            _logger.LogWarning("User {UserId} attempted to update feed {FeedId} without permission.", request.UserId, feed.Id);
            throw new BadRequestException("User does not have permission to update this feed.");
        }
    }

    private void FillFeedByRequest(Domain.Feed feed, UpdateFeedCommand request) 
    {
        if (request.Title is not null) { 
            feed.Title = request.Title;
        }
        if (request.Content is not null) { 
            feed.Content = request.Content;
        }
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
