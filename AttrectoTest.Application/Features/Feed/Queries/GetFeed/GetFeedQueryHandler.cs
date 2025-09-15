using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Mappers;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.GetFeed;

internal class GetFeedQueryHandler(IFeedRepository feedRepository, FeedMapper mapper) : IRequestHandler<GetFeedQuery, FeedDto>
{
    public async Task<FeedDto> Handle(GetFeedQuery request, CancellationToken cancellationToken)
    {
        var feed = await feedRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Feed), request.Id);
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot retrieve a deleted feed.");
        }
        return mapper.MapFeedToDto(feed, request.UserId);
    }
}
