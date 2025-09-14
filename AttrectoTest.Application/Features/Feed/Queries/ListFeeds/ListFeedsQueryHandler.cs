using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Models;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.GetFeed;

internal class ListFeedsQueryHandler : IRequestHandler<ListFeedsQuery, PagedFeeds>
{
    private readonly IFeedRepository _feedRepository;
    private readonly IAuthUserService _authUserService;

    public ListFeedsQueryHandler(IFeedRepository feedRepository, IAuthUserService authUserService)
    {
        _feedRepository = feedRepository;
        _authUserService = authUserService;
    }

    public async Task<PagedFeeds> Handle(ListFeedsQuery request, CancellationToken cancellationToken)
    {
        var feeds = _feedRepository.List().Where(x => !x.IsDeleted);
        if (request.Page is not null && request.PageSize is not null)
        {
            feeds = feeds.Skip((request.Page.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value);
        }
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
            IsOwnFeed = feed.AuthorId == _authUserService.UserId
        }).ToList();
        var result = new PagedFeeds(items, request.Page, request.PageSize, items.Count);
        return result;      
    }
}
