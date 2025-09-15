using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.CreateFeed;

public record CreateImageFeedCommand : CreateFeedCommand, IRequest<CreateFeedCommandResponse>
{
    public byte[] ImageData { get; set; } = default!;
}
