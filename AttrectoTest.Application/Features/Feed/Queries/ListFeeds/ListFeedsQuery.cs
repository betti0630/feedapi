using AttrectoTest.Application.Contracts.Requests;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Queries.Base;
using AttrectoTest.Application.Models;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Queries.ListFeeds;

public record ListFeedsQuery : ListBaseQuery, IRequest<PagedFeeds>, IBaseUrlAwareRequest
{
    public bool? IncludeExternal { get; set; }
    public ListSort? Sort { get; set; }
    public string? BaseUrl { get; set; }
}
