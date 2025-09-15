using AttrectoTest.Application.Features.Base;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.AddLike;

public record AddLikeCommand : UserRequest, IRequest
{
    public int FeedId { get; set; }
}
