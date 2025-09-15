using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;

public record UpdateVideoFeedCommand : UpdateImageFeedCommand, IRequest<UpdateFeedCommandResponse>
{
    public string? VideoUrl { get; set; }
}
