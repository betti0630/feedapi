using AttrectoTest.Application.Contracts.Requests;


using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.CreateFeed;

public record CreateImageFeedCommand : CreateFeedCommand, IRequest<CreateFeedCommandResponse>
{
    public string ImageUrl { get; set; } = default!;
}
