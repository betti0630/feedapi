using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.Application.Features.Feed.Dtos;

public abstract class PagedDto<T>
{
    public PagedDto(List<T> @items, int? @page, int? @pageSize, int @total)
    {
        this.Items = @items;
        this.Page = @page;
        this.PageSize = @pageSize;
        this.Total = @total;
    }
    [Required]
    public List<T> Items { get; }
    public int? Page { get; }
    public int? PageSize { get; }
    public int Total { get; }
}

