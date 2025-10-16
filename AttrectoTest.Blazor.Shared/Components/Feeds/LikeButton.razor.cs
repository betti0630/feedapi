using AttrectoTest.Blazor.Shared.Contracts;

using Microsoft.AspNetCore.Components;

namespace AttrectoTest.Blazor.Shared.Components.Feeds;

public partial class LikeButton : ComponentBase
{
    [Parameter]
    public int FeedId { get; set; }

    [Parameter]
    public int LikeCount { get; set; }

    [Parameter]
    public bool IsLiked { get; set; }

    [Inject] IFeedService FeedService { get; set; } = default!;

    public async void ToggleLike()
    {
        if (!IsLiked) {
            if (await FeedService.AddLike(FeedId)) {
                IsLiked = true;
                LikeCount++;
            }
        } else
        {
            if (await FeedService.DeleteLike(FeedId)) {
                IsLiked = false;
                LikeCount--;
            }
        }
        StateHasChanged();
    }
}
