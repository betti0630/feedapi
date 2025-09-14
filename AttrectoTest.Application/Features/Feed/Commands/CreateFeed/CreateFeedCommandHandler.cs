using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.CreateFeed;

internal class CreateFeedCommandHandler : 
    IRequestHandler<CreateFeedCommand, FeedDto>,
    IRequestHandler<CreateImageFeedCommand, FeedDto>
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

    public async Task<FeedDto> Handle(CreateFeedCommand request, CancellationToken cancellationToken)
    {
        var feed = ValidateAndMakeFeed<Domain.Feed>(request);
        await _feedRepository.CreateAsync(feed);
        _logger.LogInformation("Feed {FeedId} created successfully by user {UserId}.", feed.Id, _authService.UserId ?? -1);
        return MapToFeedDto(feed);
    }

    public async Task<FeedDto> Handle(CreateImageFeedCommand request, CancellationToken cancellationToken)
    {
        var feed = ValidateAndMakeFeed<Domain.ImageFeed>(request);
        feed.ImageData = request.ImageData;
        await _feedRepository.CreateImageFeedAsync(feed);
        _logger.LogInformation("Image feed {FeedId} created successfully by user {UserId}.", feed.Id, _authService.UserId ?? -1);
        return MapToFeedDto(feed);
    }

    private TFeed ValidateAndMakeFeed<TFeed>(CreateFeedCommand request) where TFeed : Domain.Feed
    {
        var validator = new CreateFeedCommandValidator();
        var validationResult = validator.Validate(request);
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
