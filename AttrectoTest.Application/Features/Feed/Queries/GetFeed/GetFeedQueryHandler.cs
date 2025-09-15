using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.GetFeed;

internal class GetFeedQueryHandler(IFeedRepository feedRepository) : IRequestHandler<GetFeedQuery, FeedDto>
{
    public async Task<FeedDto> Handle(GetFeedQuery request, CancellationToken cancellationToken)
    {
        var feed = await feedRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Feed), request.Id);
        if (feed.IsDeleted)
        {
            throw new BadRequestException("Cannot retrieve a deleted feed.");
        }
        return new FeedDto
        {
            Id = feed.Id,
            Title = feed.Title,
            Content = feed.Content,
            AuthorId = feed.AuthorId,
            AuthorUserName = feed.Author?.UserName ?? "Unknown",
            PublishedAt = feed.PublishedAt,
            IsOwnFeed = feed.AuthorId == request.UserId
        };
    }
}
