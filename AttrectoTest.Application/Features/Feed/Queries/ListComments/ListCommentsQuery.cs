using AttrectoTest.Application.Features.Feed.Dtos;


using MediatR;

namespace AttrectoTest.Application.Features.Feed.Queries.ListComments;

public class ListCommentsQuery : ListBaseQuery, IRequest<PagedComments>
{
    public int FeedId { get; set; }
}
