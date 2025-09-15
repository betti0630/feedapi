using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.Application.Features.Feed.Dtos;

public class PagedFeeds(List<FeedDto> @items, int? @page, int? @pageSize, int @total) : PagedDto<FeedDto>(items, page, pageSize, total)
{
}
