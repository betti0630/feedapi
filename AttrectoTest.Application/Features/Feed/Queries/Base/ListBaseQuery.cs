using AttrectoTest.Application.Features.Base;

namespace AttrectoTest.Application.Features.Feed.Queries.Base;

public abstract record ListBaseQuery : UserRequest
{
    public int? Page { get; set; } 
    public int? PageSize { get; set; }

}
