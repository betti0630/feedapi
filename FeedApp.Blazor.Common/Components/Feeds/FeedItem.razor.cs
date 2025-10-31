using FeedApp.Blazor.Common.Contracts;
using FeedApp.Blazor.Common.Models;

using Microsoft.AspNetCore.Components;

namespace FeedApp.Blazor.Common.Components.Feeds;

public partial class FeedItem : ComponentBase
{
    [Parameter]
    public FeedItemModel? Feed { get; set; }

    [Parameter]
    public EventCallback<int> OnFeedDeleted { get; set; }

    [Inject] protected IFeedService FeedService { get; set; } = default!;

    private async void HandleDelete()
    {
        if (Feed != null)
        {
            await FeedService.DeleteFeed(Feed.Id);
            await OnFeedDeleted.InvokeAsync(Feed.Id);
        }
    }
}
