using AttrectoTest.Blazor.Shared.Contracts;
using AttrectoTest.Blazor.Shared.Models;

using Microsoft.AspNetCore.Components;
namespace AttrectoTest.Blazor.Shared.Components.Feeds;

public partial class FeedList 
{
    [Inject] protected IFeedService FeedService { get; set; }

    private FeedListModel? _model;

    protected override async Task OnInitializedAsync()
    {
        _model = await FeedService.GetFeeds();
    }
}