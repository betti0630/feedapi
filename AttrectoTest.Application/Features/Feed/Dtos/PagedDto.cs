using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.Application.Features.Feed.Dtos;

public abstract class PagedDto<T>(List<T> @items, int? @page, int? @pageSize, int @total)
{
    [Required]
    public List<T> Items { get; } = @items;
    public int? Page { get; } = @page;
    public int? PageSize { get; } = @pageSize;
    public int Total { get; } = @total;
}

