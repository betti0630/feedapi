using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Quries.GetFeed;

public class GetFeedQuery : IRequest<FeedDto>
{
    public int Id { get; set; }
}
