
using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands;

public class CreateFeedCommand : IRequest<int>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
