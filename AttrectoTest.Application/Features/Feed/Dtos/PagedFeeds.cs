using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.Application.Features.Feed.Dtos;

public class PagedFeeds
{
    public PagedFeeds(List<FeedDto> @items, int? @page, int? @pageSize, int @total)
    {
        this.Items = @items;
        this.Page = @page;
        this.PageSize = @pageSize;
        this.Total = @total;
    }

    [Required]
    public List<FeedDto> Items { get; }

    public int? Page { get; }

    public int? PageSize { get; }

    public int Total { get; }
}
