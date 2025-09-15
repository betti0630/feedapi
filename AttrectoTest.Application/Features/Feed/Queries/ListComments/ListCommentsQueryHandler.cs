using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Queries.Base;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.ListComments;

internal class ListCommentsQueryHandler(IFeedRepository feedRepository) : ListBaseQueryHandler<ListCommentsQuery>, IRequestHandler<ListCommentsQuery, PagedComments>
{
    public async Task<PagedComments> Handle(ListCommentsQuery request, CancellationToken cancellationToken)
    {
        var feeds = feedRepository.List().Where(x => !x.IsDeleted);
        feeds = AddPaging(feeds, request);
       
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
        return null;      
    }
}
