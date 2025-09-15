using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.CreateFeed;

public record CreateVideoFeedCommand : CreateImageFeedCommand, IRequest<CreateFeedCommandResponse>
{
    public string VideoUrl { get; set; } = default!;
}
