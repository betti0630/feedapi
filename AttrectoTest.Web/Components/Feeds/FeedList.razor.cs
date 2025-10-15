
using AttrectoTest.Web.Contracts;
using AttrectoTest.Web.Services.Base;

using Microsoft.AspNetCore.Components;

namespace AttrectoTest.Web.Components.Feeds;

public partial class FeedList : ComponentBase
{
    [Inject] protected IFeedService FeedService { get; set; }

    private PagedFeeds? _feeds;

    protected override async Task OnInitializedAsync()
    {
        _feeds = await FeedService.GetFeeds();
    }
}