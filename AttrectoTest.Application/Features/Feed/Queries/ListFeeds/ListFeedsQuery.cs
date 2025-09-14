using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Queries.ListComments;
using AttrectoTest.Application.Models;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Queries.ListFeeds;

public class ListFeedsQuery : ListBaseQuery, IRequest<PagedFeeds>
{
    public bool? IncludeExternal { get; set; }
    public ListSort? Sort { get; set; }
}
