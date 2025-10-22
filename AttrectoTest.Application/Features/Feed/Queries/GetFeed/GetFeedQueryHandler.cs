using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Mappers;


using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.GetFeed;

#pragma warning disable CA1812
internal class GetFeedQueryHandler(IFeedRepository feedRepository, IIamService iamService) : IRequestHandler<GetFeedQuery, FeedDto>
{
    public async Task<FeedDto> Handle(GetFeedQuery request, CancellationToken cancellationToken)
    {
        var feeds = feedRepository.List();

        var item = feeds
            .Where(x => x.Id == request.Id)
            .Select(f => new
            {
                feed = f,
                likeCount = f.Likes.Count,
                isLiked = f.Likes.Any(c => c.UserId == request.UserId)
            })
            .FirstOrDefault()
            ?? throw new NotFoundException(nameof(Feed), request.Id);
        var feed = item.feed;
        if (feed.IsDeleted)
        {
            throw new NotFoundException(nameof(Feed), request.Id);
        }

        var result = await feed.MapFeedToDto(item.likeCount, item.isLiked, request.UserId, iamService).ConfigureAwait(false);
        return result;
    }
}
