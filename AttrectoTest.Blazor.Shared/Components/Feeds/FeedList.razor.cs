using AttrectoTest.Blazor.Shared.Contracts;
using AttrectoTest.Blazor.Shared.Models;

using Microsoft.AspNetCore.Components;
namespace AttrectoTest.Blazor.Shared.Components.Feeds;

public partial class FeedList
{
    [Inject] protected IFeedService FeedService { get; set; } = default!;

    private FeedListModel? _model;

    protected override async Task OnInitializedAsync()
    {
        _model = await FeedService.GetFeeds();
    }

    private void HandleFeedDeleted(int feedId)
    {
        if (_model?.Items != null)
        {
            var deleted = _model.Items.FirstOrDefault(f => f.Id == feedId);
            if (deleted != null)
            {
                _model.Items.Remove(deleted);
                StateHasChanged();
            }
        }
    }
}