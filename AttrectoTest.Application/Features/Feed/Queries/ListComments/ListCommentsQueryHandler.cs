using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Queries.Base;
using AttrectoTest.Domain;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.ListComments;

internal class ListCommentsQueryHandler(IFeedRepository feedRepository) : ListBaseQueryHandler<ListCommentsQuery>, IRequestHandler<ListCommentsQuery, PagedComments>
{
    public async Task<PagedComments> Handle(ListCommentsQuery request, CancellationToken cancellationToken)
    {
        var feeds = feedRepository.List();
        var feedItem = feeds
            .Where(x => x.Id == request.FeedId)
            .Select(f => new { feed = f, comments = f.Comments }).FirstOrDefault()
            ?? throw new NotFoundException(nameof(Feed), request.FeedId);
        var feed = feedItem.feed;
        if (feed.IsDeleted)
        {
            throw new KeyNotFoundException($"Feed with id {request.FeedId} not found");
        }
        var comments = feedItem.comments.AsQueryable();
        comments = AddPaging(comments, request);
       
        var items = comments.ToList()
            .Select(comment => new CommentDto(feed.Id, comment.Id, comment.Content, comment.DateCreated, comment.DateModified, comment.UserId, request.UserId))
            .ToList();
        return new PagedComments(items, request.Page, request.PageSize, feedItem.comments.Count);
    }
}
