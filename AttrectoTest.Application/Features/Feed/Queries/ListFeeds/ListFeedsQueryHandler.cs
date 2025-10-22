using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Mappers;
using AttrectoTest.Application.Features.Feed.Queries.Base;
using AttrectoTest.Application.Models;
using AttrectoTest.Application.Services;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.ListFeeds;

internal sealed class ListFeedsQueryHandler(IFeedRepository feedRepository, RssService rssService, IIamService iamService) : ListBaseQueryHandler<ListFeedsQuery>, IRequestHandler<ListFeedsQuery, PagedFeeds>
{
    public async Task<PagedFeeds> Handle(ListFeedsQuery request, CancellationToken cancellationToken)
    {
        var feeds = feedRepository.List().Where(x => !x.IsDeleted);
        var feedsWithLikes = feeds
            .Select(f => new
            {
                feed = f,
                likeCount = f.Likes.Count,
                isLiked = f.Likes.Any(c => c.UserId == request.UserId)
            });

        var feedsOrdered = request.Sort switch
        {
            ListSort.CreatedAtAsc => feedsWithLikes.OrderBy(x => x.feed.PublishedAt),
            ListSort.CreatedAtDesc => feedsWithLikes.OrderByDescending(x => x.feed.PublishedAt),
            ListSort.LikesDesc => feedsWithLikes.OrderByDescending(x => x.likeCount),
            ListSort.LikesAsc => feedsWithLikes.OrderBy(x => x.likeCount),
            _ => throw new BadRequestException("Invalid sort option.")
        };

        List<FeedDto>? items = null;
        if (request.Page.HasValue && request.PageSize.HasValue)
        {
            var skip = (request.Page.Value - 1) * request.PageSize.Value;
            var feedsPaged = feedsOrdered.Skip(skip).Take(request.PageSize.Value);
            var feedData = feedsPaged.ToList();
            var items0 = await Task.WhenAll(feedData.Select(async f =>
                await f.feed.MapFeedToDto(f.likeCount, f.isLiked, request.UserId, iamService).ConfigureAwait(false)
            )).ConfigureAwait(false);
            items = items0.ToList();
        } else { 
            var feedData = feedsOrdered.ToList();
            var items0 = await Task.WhenAll(feedData.Select(async f =>
                await f.feed.MapFeedToDto(f.likeCount, f.isLiked, request.UserId, iamService).ConfigureAwait(false)
            )).ConfigureAwait(false);
            items = items0.ToList();
        }
        if (request.IncludeExternal ?? false)
        {
            var rssItems = await rssService.GetLoveMeowFeedAsync(cancellationToken).ConfigureAwait(false);
            items.AddRange(rssItems);
        }


        var result = new PagedFeeds(items.AsReadOnly(), request.Page, request.PageSize, items.Count);
        return result;
    }
}
