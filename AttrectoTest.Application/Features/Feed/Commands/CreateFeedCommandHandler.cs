using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands;

internal class CreateFeedCommandHandler : IRequestHandler<CreateFeedCommand, int>
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

    public async Task<int> Handle(CreateFeedCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateFeedCommandValidator();
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

        var feed = new Domain.Feed
        {
            Title = request.Title,
            Content = request.Content,
            AuthorId = userId.Value
        };
        await _feedRepository.CreateAsync(feed);
        return feed.Id;
    }
}
