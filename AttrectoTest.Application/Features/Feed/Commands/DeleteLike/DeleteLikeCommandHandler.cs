using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Commands.DeleteLike;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.AddLike;

internal class DeleteLikeCommandHandler : IRequestHandler<DeleteLikeCommand>
{
    private readonly IFeedRepository _feedRepository;
    private readonly IAppLogger<AddLikeCommandHandler> _logger;
    private readonly IAuthUserService _authService;

    public DeleteLikeCommandHandler(IFeedRepository feedRepository, IAuthUserService authService,
        IAppLogger<AddLikeCommandHandler> logger)
    {
        _feedRepository = feedRepository;
        _authService = authService;
        _logger = logger;
    }

    public async Task Handle(DeleteLikeCommand request, CancellationToken cancellationToken)
    {

        var feed = await _feedRepository.GetByIdAsync(request.FeedId);
        if (feed == null)
        {
            throw new NotFoundException(nameof(Domain.Feed), request.FeedId);
        }
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot update a deleted feed.");
        }
        var userId = _authService.UserId;
        _logger.LogInformation("Feed {FeedId} liked successfully by user {UserId}.", feed.Id, userId ?? -1);
    }
}
