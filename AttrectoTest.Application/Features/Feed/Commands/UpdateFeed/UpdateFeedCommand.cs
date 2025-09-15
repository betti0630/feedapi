using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;

public record UpdateFeedCommand : IRequest<UpdateFeedCommandResponse>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
}
