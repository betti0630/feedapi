using AttrectoTest.Blazor.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace AttrectoTest.Blazor.Shared.Components.Feeds;

public partial class FeedItem : ComponentBase
{
    [Parameter]
    public FeedItemModel? Feed { get; set;}

    private void HandleDelete()
    {

    }
}
