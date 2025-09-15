using AttrectoTest.Domain;

namespace AttrectoTest.Application.Contracts.Persistence;

public interface IFeedRepository: IGenericRepository<Feed>
{
    Task<int> GetLikesCountAsync(int feedId, CancellationToken cancellationToken = default);
    Task AddLikeAsync(FeedLike feedLike, CancellationToken cancellationToken = default);
    Task RemoveLikeAsync(FeedLike feedLike, CancellationToken cancellationToken = default);
}
