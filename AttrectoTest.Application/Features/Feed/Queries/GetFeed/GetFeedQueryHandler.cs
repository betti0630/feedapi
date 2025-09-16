using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Mappers;
using AttrectoTest.Domain;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.GetFeed;

internal class GetFeedQueryHandler(IFeedRepository feedRepository, FeedMapper mapper) : IRequestHandler<GetFeedQuery, FeedDto>
{
    public async Task<FeedDto> Handle(GetFeedQuery request, CancellationToken cancellationToken)
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

        return mapper.MapFeedToDto(feed, item.likeCount, request.UserId, request.BaseUrl);
    }
}
