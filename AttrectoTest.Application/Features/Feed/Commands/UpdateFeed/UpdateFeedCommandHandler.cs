using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Domain;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;

internal class UpdateFeedCommandHandler : 
    IRequestHandler<UpdateFeedCommand, FeedDto>,
    IRequestHandler<UpdateImageFeedCommand, FeedDto>,
    IRequestHandler<UpdateVideoFeedCommand, FeedDto>
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
        var feed =await ValidateRequestAndGetUpdatedFeed(request, (id) => _feedRepository.GetByIdAsync(id));

        await _feedRepository.UpdateAsync(feed);
        _logger.LogInformation("Feed {FeedId} updated successfully by user {UserId}.", feed.Id, _authService.UserId ?? -1);
        return MapToFeedDto(feed);
    }

    public async Task<FeedDto> Handle(UpdateImageFeedCommand request, CancellationToken cancellationToken)
    {
        var feed = await ValidateRequestAndGetUpdatedFeed(request, async (id) => await _feedRepository.GetImageFeedByIdAsync(id)) as ImageFeed;
        if (feed == null)
        {
            throw new BadRequestException("Feed is not an image feed.");
        }
        feed.ImageData = request.ImageData;
        await _feedRepository.UpdateImageFeedAsync(feed);
        _logger.LogInformation("Image feed {FeedId} updated successfully by user {UserId}.", feed.Id, _authService.UserId ?? -1);
        return MapToFeedDto(feed);
    }

    public async Task<FeedDto> Handle(UpdateVideoFeedCommand request, CancellationToken cancellationToken)
    {
        var feed = await ValidateRequestAndGetUpdatedFeed(request, async (id) => await _feedRepository.GetVideoFeedByIdAsync(id)) as VideoFeed;
        if (feed == null)
        {
            throw new BadRequestException("Feed is not a video feed.");
        }
        feed.ImageData = request.ImageData;
        feed.VideoUrl = request.VideoUrl;
        await _feedRepository.UpdateVideoFeedAsync(feed);
        _logger.LogInformation("Video feed {FeedId} updated successfully by user {UserId}.", feed.Id, _authService.UserId ?? -1);
        return MapToFeedDto(feed);
    }

    private async Task<Domain.Feed> ValidateRequestAndGetUpdatedFeed(UpdateFeedCommand request, Func<int, Task<Domain.Feed?>> getFeedFunc)
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
        var feed = await getFeedFunc(request.Id);
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
        return feed;
    }

    private FeedDto MapToFeedDto(Domain.Feed feed)
    {
        return new FeedDto
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
