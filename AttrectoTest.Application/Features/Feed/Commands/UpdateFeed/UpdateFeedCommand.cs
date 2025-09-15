using AttrectoTest.Application.Features.Base;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;

public record UpdateFeedCommand : UserRequest, IRequest<UpdateFeedCommandResponse>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
}
