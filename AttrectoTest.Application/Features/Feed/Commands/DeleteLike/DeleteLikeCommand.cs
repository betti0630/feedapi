using AttrectoTest.Application.Features.Base;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteLike;

public record DeleteLikeCommand : UserRequest, IRequest
{
    public int FeedId { get; set; }
}
