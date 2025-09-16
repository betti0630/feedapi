using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.PurgeFeed;

internal class PurgeFeedCommandHandler(IFeedRepository feedRepository, IAppLogger<PurgeFeedCommandHandler> logger) : IRequestHandler<PurgeFeedCommand>
{
    public async Task Handle(PurgeFeedCommand request, CancellationToken cancellationToken)
    {
        var feed = feedRepository.List().Where(f => f.IsDeleted).ToList();
        foreach(var entry in feed) {
            await feedRepository.DeleteAsync(entry, cancellationToken);
        }
        logger.LogInformation("Feeds delete was successful");
    }
}
