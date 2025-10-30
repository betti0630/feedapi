using AttrectoTest.Blazor.Common.Contracts;
using AttrectoTest.BlazorWeb.Services.Notification;

using Microsoft.AspNetCore.Components;

namespace AttrectoTest.Blazor.Common.Components.Feeds;

public partial class LikeButton : ComponentBase
{
    [Parameter]
    public int FeedId { get; set; }

    [Parameter]
    public int LikeCount { get; set; }

    [Parameter]
    public bool IsLiked { get; set; }

    [Parameter]
    public int AuthorId { get; set; }

    [Inject] IFeedService FeedService { get; set; } = default!;

    [Inject] IFeedNotificationService FeedNotificationService { get; set; } = default!;

    public async void ToggleLike()
    {
        if (!IsLiked) {
            if (await FeedService.AddLike(FeedId)) {
                IsLiked = true;
                LikeCount++;
                await FeedNotificationService.CreateAsync(new Notification
                {
                    UserId = AuthorId,
                    Message = $"Valaki lájkolta a bejegyzésedet."
                });
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
