using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Mappers;


using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.GetFeed;

internal class GetFeedQueryHandler(IFeedRepository feedRepository) : IRequestHandler<GetFeedQuery, FeedDto>
{
    public Task<FeedDto> Handle(GetFeedQuery request, CancellationToken cancellationToken)
    {
        var feeds = feedRepository.List();

        var item = feeds
            .Where(x => x.Id == request.Id)
            .Select(f => new { feed = f, likeCount = f.Likes.Count() }).FirstOrDefault()
            ?? throw new NotFoundException(nameof(Feed), request.Id); 
        var feed = item.feed;
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot retrieve a deleted feed.");
        }

        var result = feed.MapFeedToDto(item.likeCount, request.UserId);
        return Task.FromResult(result);
    }
}
