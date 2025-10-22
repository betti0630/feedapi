using AttrectoTest.Blazor.Common.Contracts;
using AttrectoTest.Blazor.Common.Models;

using Microsoft.AspNetCore.Components;

namespace AttrectoTest.Blazor.Common.Components.Feeds;

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
