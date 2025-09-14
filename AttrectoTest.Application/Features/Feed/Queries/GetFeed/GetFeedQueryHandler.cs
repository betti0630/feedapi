using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Queries.GetFeed;

internal class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, FeedDto>
{
    private readonly IFeedRepository _feedRepository;
    private readonly IAuthUserService _authUserService;

    public GetFeedQueryHandler(IFeedRepository feedRepository, IAuthUserService authUserService)
    {
        _feedRepository = feedRepository;
        _authUserService = authUserService;
    }

    public async Task<FeedDto> Handle(GetFeedQuery request, CancellationToken cancellationToken)
    {
        var feed = await _feedRepository.GetByIdAsync(request.Id);
        if (feed == null)
        {
            throw new NotFoundException(nameof(Feed), request.Id);
        }
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
            IsOwnFeed = feed.AuthorId == _authUserService.UserId
        };
    }
}
