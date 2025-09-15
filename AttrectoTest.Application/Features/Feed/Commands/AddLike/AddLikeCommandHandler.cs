using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.AddLike;

internal class AddLikeCommandHandler : IRequestHandler<AddLikeCommand>
{
    private readonly IFeedRepository _feedRepository;
    private readonly IAppLogger<AddLikeCommandHandler> _logger;


    public AddLikeCommandHandler(IFeedRepository feedRepository, IAppLogger<AddLikeCommandHandler> logger)
    {
        _feedRepository = feedRepository;
        _logger = logger;
    }

    public async Task Handle(AddLikeCommand request, CancellationToken cancellationToken)
    {

        var feed = await _feedRepository.GetByIdAsync(request.FeedId) ?? throw new NotFoundException(nameof(Domain.Feed), request.FeedId);
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot update a deleted feed.");
        }
        _logger.LogInformation("Feed {FeedId} liked successfully by user {UserId}.", feed.Id, request.UserId);
    }
}
