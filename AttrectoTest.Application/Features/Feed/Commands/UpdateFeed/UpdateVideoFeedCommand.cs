using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;

public record UpdateVideoFeedCommand : UpdateImageFeedCommand, IRequest<FeedDto>
{
    public string VideoUrl { get; set; } = default!;
}
