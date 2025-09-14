using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteFeed;

public record DeleteFeedCommand : IRequest
{
    public int Id { get; set; }
}
