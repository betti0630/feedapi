using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.CreateFeed;

internal class CreateFeedCommandHandler : 
    IRequestHandler<CreateFeedCommand, CreateFeedCommandResponse>,
    IRequestHandler<CreateImageFeedCommand, CreateFeedCommandResponse>,
    IRequestHandler<CreateVideoFeedCommand, CreateFeedCommandResponse>
{
    private readonly IFeedRepository _feedRepository;
    private readonly IAppLogger<CreateFeedCommandHandler> _logger;
    private readonly IAuthUserService _authService;

    public CreateFeedCommandHandler(IFeedRepository feedRepository, IAuthUserService authService,
        IAppLogger<CreateFeedCommandHandler> logger)
    {
        _feedRepository = feedRepository;
        _authService = authService;
        _logger = logger;
    }

    public async Task<CreateFeedCommandResponse> Handle(CreateFeedCommand request, CancellationToken cancellationToken)
    {
        var feed = await ValidateAndMakeFeed<Domain.Feed>(request);
        await _feedRepository.CreateAsync(feed);
        _logger.LogInformation("Feed {FeedId} created successfully by user {UserId}.", feed.Id, _authService.UserId ?? -1);
        return MapToResponse(feed);
    }

    public async Task<CreateFeedCommandResponse> Handle(CreateImageFeedCommand request, CancellationToken cancellationToken)
    {
        var feed = await ValidateAndMakeFeed<Domain.ImageFeed>(request);
        feed.ImageData = request.ImageData;
        await _feedRepository.CreateAsync(feed);
        _logger.LogInformation("Image feed {FeedId} created successfully by user {UserId}.", feed.Id, _authService.UserId ?? -1);
        return MapToResponse(feed);
    }

    public async Task<CreateFeedCommandResponse> Handle(CreateVideoFeedCommand request, CancellationToken cancellationToken)
    {
        var feed = await ValidateAndMakeFeed<Domain.VideoFeed>(request);
        feed.ImageData = request.ImageData;
        feed.VideoUrl = request.VideoUrl;
        await _feedRepository.CreateAsync(feed);
        _logger.LogInformation("Video feed {FeedId} created successfully by user {UserId}.", feed.Id, _authService.UserId ?? -1);
        return MapToResponse(feed);
    }

    private async Task<TFeed> ValidateAndMakeFeed<TFeed>(CreateFeedCommand request) where TFeed : Domain.Feed
    {
        var validator = new CreateFeedCommandValidator();
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
        var feed = Activator.CreateInstance<TFeed>() as Domain.Feed;

        feed.Title = request.Title;
        feed.Content = request.Content;
        feed.AuthorId = userId.Value;

        return (TFeed)feed;

    }

    private CreateFeedCommandResponse MapToResponse(Domain.Feed feed)
    {
        return new CreateFeedCommandResponse
        {
            Id = feed.Id,
            Title = feed.Title,
            PublishedAt = feed.PublishedAt
        };
    }


}
