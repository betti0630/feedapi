using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Queries.Base;
using AttrectoTest.Application.Features.Feed.Queries.ListFeeds;
using AttrectoTest.Application.Models;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.ListComments;

internal class ListCommentsQueryHandler : ListBaseQueryHandler<ListCommentsQuery>, IRequestHandler<ListCommentsQuery, PagedComments>
{
    private readonly IFeedRepository _feedRepository;
    private readonly IAuthUserService _authUserService;

    public ListCommentsQueryHandler(IFeedRepository feedRepository, IAuthUserService authUserService)
    {
        _feedRepository = feedRepository;
        _authUserService = authUserService;
    }

    public async Task<PagedComments> Handle(ListCommentsQuery request, CancellationToken cancellationToken)
    {
        var feeds = _feedRepository.List().Where(x => !x.IsDeleted);
        feeds = AddPaging(feeds, request);
       
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
        return null;      
    }
}
