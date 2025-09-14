using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.Application.Features.Feed.Dtos;

public class PagedFeeds : PagedDto<FeedDto>
{
    public PagedFeeds(List<FeedDto> @items, int? @page, int? @pageSize, int @total) : base(items, page, pageSize, total)
    {
    }

}
