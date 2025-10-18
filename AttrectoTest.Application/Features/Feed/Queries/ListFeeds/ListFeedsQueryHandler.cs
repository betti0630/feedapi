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

internal class ListFeedsQueryHandler(IFeedRepository feedRepository, RssService rssService, IAimService aimService) : ListBaseQueryHandler<ListFeedsQuery>, IRequestHandler<ListFeedsQuery, PagedFeeds>
{
    public async Task<PagedFeeds> Handle(ListFeedsQuery request, CancellationToken cancellationToken)
    {
        var feeds = feedRepository.List().Where(x => !x.IsDeleted);
        feeds = AddPaging<Domain.Feed>(feeds, request);

        var feedData = feeds
            .Select(f => new
            {
                feed = f,
                likeCount = f.Likes.Count(),
                isLiked = f.Likes.Count(c => c.UserId == request.UserId) > 0
            })
            .ToList();
        var items0 = await Task.WhenAll(feedData.Select(async f =>
            await f.feed.MapFeedToDto(f.likeCount, f.isLiked, request.UserId, aimService)
        ));
        var items = items0.ToList();
        if (request.IncludeExternal ?? false)
        {
            var rssItems = await rssService.GetLoveMeowFeedAsync(cancellationToken);
            items.AddRange(rssItems);
        }

        items = [.. (request.Sort switch
        {
            ListSort.CreatedAt_asc => items.OrderBy(x => x.PublishedAt),
            ListSort.CreatedAt_desc => items.OrderByDescending(x => x.PublishedAt),
            ListSort.Likes_desc => items.OrderByDescending(x => x.LikeCount),
            ListSort.Likes_asc => items.OrderBy(x => x.LikeCount),
            _ => throw new BadRequestException("Invalid sort option."),
        })];
        var result = new PagedFeeds(items, request.Page, request.PageSize, items.Count);
        return result;      
    }
}
