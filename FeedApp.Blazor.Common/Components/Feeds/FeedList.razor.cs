using FeedApp.Blazor.Common.Contracts;
using FeedApp.Blazor.Common.Models;

using Microsoft.AspNetCore.Components;
namespace FeedApp.Blazor.Common.Components.Feeds;

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

    private bool IsModalVisible;

    void OpenNewFeedModal() => IsModalVisible = true;

    async void CloseNewFeedModal() {
        IsModalVisible = false;
        _model = await FeedService.GetFeeds();
        StateHasChanged();
    }

}