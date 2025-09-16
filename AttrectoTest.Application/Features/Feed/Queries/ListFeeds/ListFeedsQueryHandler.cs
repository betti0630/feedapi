using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Mappers;
using AttrectoTest.Application.Features.Feed.Queries.Base;
using AttrectoTest.Application.Models;
using AttrectoTest.Application.Services;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.ListFeeds;

internal class ListFeedsQueryHandler(IFeedRepository feedRepository, FeedMapper mapper, RssService rssService) : ListBaseQueryHandler<ListFeedsQuery>, IRequestHandler<ListFeedsQuery, PagedFeeds>
{
    public async Task<PagedFeeds> Handle(ListFeedsQuery request, CancellationToken cancellationToken)
    {
        var feeds = feedRepository.List().Where(x => !x.IsDeleted);
        feeds = AddPaging<Domain.Feed>(feeds, request);

        var items = feeds.Select(f => new {feed = f, likeCount = f.Likes.Count()}).ToList()
            .Select(f => mapper.MapFeedToDto(f.feed, f.likeCount, request.UserId)).ToList();
        var rssItems = await rssService.GetLoveMeowFeedAsync(cancellationToken);
        items.AddRange(rssItems);
        items = (request.Sort switch
        {
            ListSort.CreatedAt_asc => items.OrderBy(x => x.PublishedAt),
            ListSort.CreatedAt_desc => items.OrderByDescending(x => x.PublishedAt),
            ListSort.Likes_desc => items.OrderByDescending(x => x.LikeCount),
            ListSort.Likes_asc => items.OrderBy(x => x.LikeCount),
            _ => throw new BadRequestException("Invalid sort option."),
        }).ToList();
        var result = new PagedFeeds(items, request.Page, request.PageSize, items.Count);
        return result;      
    }
}
