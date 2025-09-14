namespace AttrectoTest.Application.Features.Feed.Dtos;

public class PagedComments : PagedDto<CommentDto>
{
    public PagedComments(List<CommentDto> @items, int? @page, int? @pageSize, int @total) : base(items, page, pageSize, total)
    {

    }

}
