using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.CreateFeed;

public record DeleteFeedCommand : IRequest
{
    public int Id { get; set; }
}
