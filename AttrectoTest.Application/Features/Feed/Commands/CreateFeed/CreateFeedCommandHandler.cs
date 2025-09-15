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

    public CreateFeedCommandHandler(IFeedRepository feedRepository, IAppLogger<CreateFeedCommandHandler> logger)
    {
        _feedRepository = feedRepository;
        _logger = logger;
    }

    public async Task<CreateFeedCommandResponse> Handle(CreateFeedCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid feed", validationResult);
        }
        
        var feed = CreateFeedFromRequest<Domain.Feed>(request);
        await _feedRepository.CreateAsync(feed);
        _logger.LogInformation("Feed {FeedId} created successfully by user {UserId}.", feed.Id, request.UserId);
        return MapToResponse(feed);
    }

    public async Task<CreateFeedCommandResponse> Handle(CreateImageFeedCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid feed", validationResult);
        }
        
        var feed = CreateFeedFromRequest<Domain.ImageFeed>(request);
        feed.ImageData = request.ImageData;
        await _feedRepository.CreateAsync(feed);
        _logger.LogInformation("Image feed {FeedId} created successfully by user {UserId}.", feed.Id, request.UserId);
        return MapToResponse(feed);
    }

    public async Task<CreateFeedCommandResponse> Handle(CreateVideoFeedCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid feed", validationResult);
        }

        var feed = CreateFeedFromRequest<Domain.VideoFeed>(request);
        feed.ImageData = request.ImageData;
        feed.VideoUrl = request.VideoUrl;
        await _feedRepository.CreateAsync(feed);
        _logger.LogInformation("Video feed {FeedId} created successfully by user {UserId}.", feed.Id, request.UserId);
        return MapToResponse(feed);
    }

    private TFeed CreateFeedFromRequest<TFeed>(CreateFeedCommand request) where TFeed : Domain.Feed
    {

        var feed = Activator.CreateInstance<TFeed>() as Domain.Feed;

        feed.Title = request.Title;
        feed.Content = request.Content;
        feed.AuthorId = request.UserId;

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
