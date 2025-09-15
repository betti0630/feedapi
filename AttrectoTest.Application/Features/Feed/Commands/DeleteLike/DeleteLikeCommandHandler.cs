using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Commands.AddLike;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteLike;

internal class DeleteLikeCommandHandler : IRequestHandler<DeleteLikeCommand>
{
    private readonly IFeedRepository _feedRepository;
    private readonly IAppLogger<AddLikeCommandHandler> _logger;

    public DeleteLikeCommandHandler(IFeedRepository feedRepository, IAppLogger<AddLikeCommandHandler> logger)
    {
        _feedRepository = feedRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteLikeCommand request, CancellationToken cancellationToken)
    {

        var feed = await _feedRepository.GetByIdAsync(request.FeedId) ?? throw new NotFoundException(nameof(Domain.Feed), request.FeedId);
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot update a deleted feed.");
        }
        _logger.LogInformation("Feed {FeedId} liked successfully by user {UserId}.", feed.Id, request.UserId);
    }
}
