using AttrectoTest.Web.Contracts;
using AttrectoTest.Web.Shared.Models;

using Microsoft.AspNetCore.Components;
namespace AttrectoTest.Web.Shared.Components.Feeds;

public partial class FeedList 
{
    [Inject] protected IFeedService FeedService { get; set; }

    private FeedListModel? _model;

    protected override async Task OnInitializedAsync()
    {
        _model = await FeedService.GetFeeds();
    }
}