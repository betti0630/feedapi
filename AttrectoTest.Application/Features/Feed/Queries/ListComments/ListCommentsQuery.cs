using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Queries.Base;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Queries.ListComments;

public record ListCommentsQuery : ListBaseQuery, IRequest<PagedComments>
{
    public int FeedId { get; set; }
}
