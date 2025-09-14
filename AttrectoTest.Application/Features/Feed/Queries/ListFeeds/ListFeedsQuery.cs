using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Models;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Queries.GetFeed;

public class ListFeedsQuery : IRequest<PagedFeeds>
{
    public int? UserId { get; set; }
    public bool? IncludeExternal { get; set; }
    public int? Page { get; set; } 
    public int? PageSize { get; set; }
    public ListSort? Sort { get; set; } 
}
