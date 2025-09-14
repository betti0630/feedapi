using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Queries.GetFeed;

public class GetFeedQuery : IRequest<FeedDto>
{
    public int Id { get; set; }
}
