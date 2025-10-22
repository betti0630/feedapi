using System.Collections.ObjectModel;

namespace AttrectoTest.Application.Features.Feed.Dtos;

public class PagedFeeds(ReadOnlyCollection<FeedDto> @items, int? @page, int? @pageSize, int @total) : PagedDto<FeedDto>(items, page, pageSize, total)
{
}
