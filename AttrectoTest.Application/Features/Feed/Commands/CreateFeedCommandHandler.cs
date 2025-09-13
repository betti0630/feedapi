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
    private readonly IAuthService _authService;

    public CreateFeedCommandHandler(IFeedRepository feedRepository, IAuthService authService,
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
        var user = await _authService.GetCurrentUser();
        var feed = new Domain.Feed
        {
            Title = request.Title,
            Content = request.Content
        };
        await _feedRepository.CreateAsync(feed);
        return feed.Id;
    }
}
