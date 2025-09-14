using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.CreateFeed;

public record CreateFeedCommand : IRequest<FeedDto>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
