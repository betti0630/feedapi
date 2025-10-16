using Microsoft.AspNetCore.Components;

namespace AttrectoTest.Blazor.Shared.Components.Feeds;

public partial class LikeButton : ComponentBase
{
    [Parameter]
    public int FeedId { get; set; }

    [Parameter]
    public int InitialCount { get; set; }

    [Parameter]
    public bool InitialLiked { get; set; }
}
