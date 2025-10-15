using AttrectoTest.Web.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace AttrectoTest.Web.Shared.Components.Feeds;

public partial class FeedItem : ComponentBase
{
    [Parameter]
    public FeedItemModel? Feed { get; set;}

    private void HandleDelete()
    {

    }
}
