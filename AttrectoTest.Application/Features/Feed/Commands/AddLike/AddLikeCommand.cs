using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.AddLike;

public class AddLikeCommand : IRequest
{
    public int FeedId { get; set; }
}
