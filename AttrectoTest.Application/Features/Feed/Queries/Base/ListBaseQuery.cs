using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Models;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Queries.ListComments;

public abstract class ListBaseQuery 
{
    public int? Page { get; set; } 
    public int? PageSize { get; set; }

}
