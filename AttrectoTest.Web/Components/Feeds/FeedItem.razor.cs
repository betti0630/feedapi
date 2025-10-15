using AttrectoTest.Web.Services.Base;

using Microsoft.AspNetCore.Components;

namespace AttrectoTest.Web.Components.Feeds;

public partial class FeedItem : ComponentBase
{
    [Parameter]
    public FeedDto? Feed { get; set;}

    private void HandleDelete()
    {

    }
}
