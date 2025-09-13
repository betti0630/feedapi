using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands;

internal class CreateFeedCommandHandler : IRequestHandler<CreateFeedCommand, int>
{
    private readonly IFeedRepository _feedRepository;
    private readonly IAppLogger<CreateFeedCommandHandler> _logger;

    public CreateFeedCommandHandler(IFeedRepository feedRepository, IAppLogger<CreateFeedCommandHandler> logger)
    {
        _feedRepository = feedRepository;
        _logger = logger;
    }

    public async Task<int> Handle(CreateFeedCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any()) { 
            throw new BadRequestException("Invalid feed", validationResult);
        }
        var feed = new Domain.Feed
        {
            Title = request.Title,
            Content = request.Content,
            CreatedBy = "test"
        };
        await _feedRepository.CreateAsync(feed);
        return feed.Id;
    }
}
