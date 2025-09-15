using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Queries.Base;
using AttrectoTest.Application.Models;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.ListFeeds;

internal class ListFeedsQueryHandler(IFeedRepository feedRepository) : ListBaseQueryHandler<ListFeedsQuery>, IRequestHandler<ListFeedsQuery, PagedFeeds>
{
    public async Task<PagedFeeds> Handle(ListFeedsQuery request, CancellationToken cancellationToken)
    {
        var feeds = feedRepository.List().Where(x => !x.IsDeleted);
        feeds = AddPaging<Domain.Feed>(feeds, request);

        switch (request.Sort) 
        {
            case ListSort.CreatedAt_asc:
                feeds = feeds.OrderBy(x => x.PublishedAt);
                break;
            case ListSort.CreatedAt_desc:
                feeds = feeds.OrderByDescending(x => x.PublishedAt);
                break;
            //case ListSort.Likes_desc:
            //    feeds = feeds.OrderByDescending(x => x.Likes.Count);
            //    break;
            //case ListSort.Likes_asc:
            //        feeds = feeds.OrderBy(x => x.Likes.Count);
            default:
                throw new BadRequestException("Invalid sort option.");
        }
        var items = feeds.Select(feed => new FeedDto()
        {
            Id = feed.Id,
            Title = feed.Title,
            Content = feed.Content,
            AuthorId = feed.AuthorId,
            AuthorUserName = feed.Author.UserName,
            PublishedAt = feed.PublishedAt,
            IsOwnFeed = feed.AuthorId == request.UserId
        }).ToList();
        var result = new PagedFeeds(items, request.Page, request.PageSize, items.Count);
        return result;      
    }
}
