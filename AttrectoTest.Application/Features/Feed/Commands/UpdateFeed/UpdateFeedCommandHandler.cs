using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Domain;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;

public class UpdateFeedCommandHandler(IFeedRepository feedRepository, IAppLogger<UpdateFeedCommandHandler> logger) : 
    IRequestHandler<UpdateFeedCommand, UpdateFeedCommandResponse>,
    IRequestHandler<UpdateImageFeedCommand, UpdateFeedCommandResponse>,
    IRequestHandler<UpdateVideoFeedCommand, UpdateFeedCommandResponse>
{
    public async Task<UpdateFeedCommandResponse> Handle(UpdateFeedCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var validator = new UpdateFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);

        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid feed", validationResult);
        }

        var feed = await feedRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false) ?? throw new NotFoundException(nameof(Domain.Feed), request.Id);
        if (feed is not Domain.Feed)
        {
            throw new BadRequestException($"Feed is not of type {nameof(Domain.Feed)}.");
        }
        ValidateFeed(feed, request);
        FillFeedByRequest(feed, request);

        await feedRepository.UpdateAsync(feed, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Feed {FeedId} updated successfully by user {AuthorId}.", feed.Id, request.UserId);
        return MapToResponse(feed);
    }

    public async Task<UpdateFeedCommandResponse> Handle(UpdateImageFeedCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var validator = new UpdateFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);

        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid feed", validationResult);
        }

        var feed = await feedRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (feed is not ImageFeed imageFeed)
        {
            throw new BadRequestException($"Feed is not of type { nameof(ImageFeed) }.");
        }
        ValidateFeed(imageFeed, request);
        FillFeedByRequest(feed, request);

        if (request.ImageUrl is not null) {
            imageFeed.ImageUrl = request.ImageUrl;
        }
        await feedRepository.UpdateAsync(imageFeed, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Image feed {FeedId} updated successfully by user {AuthorId}.", imageFeed.Id, request.UserId);
        return MapToResponse(feed);
    }

    public async Task<UpdateFeedCommandResponse> Handle(UpdateVideoFeedCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var validator = new UpdateVideoFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);

        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid feed", validationResult);
        }

        var feed = await feedRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (feed is not VideoFeed videoFeed)
        {
            throw new BadRequestException($"Feed is not of type {nameof(VideoFeed)}.");
        }
        ValidateFeed(videoFeed, request);
        FillFeedByRequest(videoFeed, request);

        if (request.ImageUrl is not null) {
            videoFeed.ImageUrl = request.ImageUrl;
        }
        if (request.VideoUrl is not null) {
            videoFeed.VideoUrl = request.VideoUrl;
        }
        await feedRepository.UpdateAsync(videoFeed, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Video feed {FeedId} updated successfully by user {AuthorId}.", videoFeed.Id, request.UserId);
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
            logger.LogWarning("User {AuthorId} attempted to update feed {FeedId} without permission.", request.UserId, feed.Id);
            throw new BadRequestException("User does not have permission to update this feed.");
        }
    }

    private static void FillFeedByRequest(Domain.Feed feed, UpdateFeedCommand request) 
    {
        if (request.Title is not null) { 
            feed.Title = request.Title;
        }
        if (request.Content is not null) { 
            feed.Content = request.Content;
        }
    }

    private static UpdateFeedCommandResponse MapToResponse(Domain.Feed feed)
    {
        return new UpdateFeedCommandResponse
        {
            Id = feed.Id,
            Title = feed.Title,
            PublishedAt = feed.PublishedAt
        };
    }
}
