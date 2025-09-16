using AttrectoTest.Application.Contracts.Requests;
using AttrectoTest.Application.Features.Base;
using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Queries.GetFeed;

public record GetFeedQuery : UserRequest, IRequest<FeedDto>, IBaseUrlAwareRequest
{
    public int Id { get; set; }
    public string? BaseUrl { get; set; }
}
