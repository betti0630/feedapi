using AttrectoTest.Domain;

namespace AttrectoTest.Application.Contracts.Persistence;

public interface IFeedRepository: IQueryableRepository<Feed>, IDisposable
{
    Task<int> GetLikesCountAsync(int feedId, CancellationToken cancellationToken = default);
    Task AddLikeAsync(int feedId, int userId, CancellationToken cancellationToken = default);
    Task RemoveLikeAsync(int feedId, int userId, CancellationToken cancellationToken = default);
}
