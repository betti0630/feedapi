using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;

public record UpdateImageFeedCommand : UpdateFeedCommand, IRequest<UpdateFeedCommandResponse>
{
    public string? ImageUrl { get; set; }
}
