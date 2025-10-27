using System.Collections.ObjectModel;

namespace AttrectoTest.Application.Features.Feed.Dtos;

public class PagedComments(List<CommentDto> @items, int? @page, int? @pageSize, int @total) : PagedDto<CommentDto>(items, page, pageSize, total)
{
}
