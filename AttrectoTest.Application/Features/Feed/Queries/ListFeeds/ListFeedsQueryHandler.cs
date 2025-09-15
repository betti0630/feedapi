using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Mappers;
using AttrectoTest.Application.Features.Feed.Queries.Base;
using AttrectoTest.Application.Models;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.ListFeeds;

internal class ListFeedsQueryHandler(IFeedRepository feedRepository, FeedMapper mapper) : ListBaseQueryHandler<ListFeedsQuery>, IRequestHandler<ListFeedsQuery, PagedFeeds>
{
    public async Task<PagedFeeds> Handle(ListFeedsQuery request, CancellationToken cancellationToken)
    {
        var feeds = feedRepository.List().Where(x => !x.IsDeleted);
        feeds = AddPaging<Domain.Feed>(feeds, request);

        feeds = request.Sort switch
        {
            ListSort.CreatedAt_asc => feeds.OrderBy(x => x.PublishedAt),
            ListSort.CreatedAt_desc => feeds.OrderByDescending(x => x.PublishedAt),
            ListSort.Likes_desc => feeds.OrderByDescending(x => x.Likes.Count),
            ListSort.Likes_asc => feeds.OrderBy(x => x.Likes.Count),
            _ => throw new BadRequestException("Invalid sort option."),
        };
        var items = feeds.Select(f => new {feed = f, likeCount = f.Likes.Count()}).ToList().Select(f => mapper.MapFeedToDto(f.feed, f.likeCount, request.UserId)).ToList();
        var result = new PagedFeeds(items, request.Page, request.PageSize, items.Count);
        return result;      
    }
}
