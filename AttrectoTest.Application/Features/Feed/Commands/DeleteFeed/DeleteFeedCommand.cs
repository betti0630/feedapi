using AttrectoTest.Application.Features.Base;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteFeed;

public record DeleteFeedCommand : UserRequest, IRequest
{
    public int Id { get; set; }
}
