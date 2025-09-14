using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteLike;

public class DeleteLikeCommand : IRequest
{
    public int FeedId { get; set; }
}
