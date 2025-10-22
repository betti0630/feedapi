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

        IQueryable<FeedWithLikes> feedsWithLikes = feeds
            .Select(f => new FeedWithLikes
            {
                Feed = f,
                LikeCount = f.Likes.Count,
                IsLiked = f.Likes.Any(c => c.UserId == request.UserId)
            });

        IQueryable<FeedWithLikes> feedsOrdered = request.Sort switch
        {
            ListSort.CreatedAtAsc => feedsWithLikes.OrderBy(x => x.Feed.PublishedAt),
            ListSort.CreatedAtDesc => feedsWithLikes.OrderByDescending(x => x.Feed.PublishedAt),
            ListSort.LikesDesc => feedsWithLikes.OrderByDescending(x => x.LikeCount),
            ListSort.LikesAsc => feedsWithLikes.OrderBy(x => x.LikeCount),
            _ => throw new BadRequestException("Invalid sort option.")
        };

        IQueryable<FeedWithLikes> feedsPaged = feedsOrdered;
        if (request.Page.HasValue && request.PageSize.HasValue)
        {
            var skip = (request.Page.Value - 1) * request.PageSize.Value;
            feedsPaged = feedsOrdered.Skip(skip).Take(request.PageSize.Value);
        }


        List<FeedWithLikes> feedData = feedsPaged.ToList();

    
        FeedDto[]? items0 = await Task.WhenAll(feedData.Select(async f =>
            await f.Feed.MapFeedToDto(f.LikeCount, f.IsLiked, request.UserId, iamService).ConfigureAwait(false)
        )).ConfigureAwait(false);
        List<FeedDto>? items = items0.ToList();

        List<RssFeedDto>? rssItems = null;
        if (request.IncludeExternal ?? false)
        {
            rssItems = await rssService.GetLoveMeowFeedAsync(cancellationToken).ConfigureAwait(false);
            items.AddRange(rssItems);
            items = request.Sort switch
            {
                ListSort.CreatedAtAsc => items.OrderBy(x => x.PublishedAt).ToList(),
                ListSort.CreatedAtDesc => items.OrderByDescending(x => x.PublishedAt).ToList(),
                ListSort.LikesDesc => items.OrderByDescending(x => x.LikeCount).ToList(),
                ListSort.LikesAsc => items.OrderBy(x => x.LikeCount).ToList(),
                _ => throw new BadRequestException("Invalid sort option.")
            };
            if (request.Page.HasValue && request.PageSize.HasValue)
            {
                var skip = (request.Page.Value - 1) * request.PageSize.Value;
                items = items.Skip(skip).Take(request.PageSize.Value).ToList();
            }

        }



        var result = new PagedFeeds(items.AsReadOnly(), request.Page, request.PageSize, items.Count);
        return result;
    }

    private class FeedWithLikes
    {
        public Domain.Feed Feed { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
    }
}
