using HR.LeaveManagement.BlazorUI.Providers;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AttrectoTest.Web.Components.Pages;

public partial class Home
{
    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    protected async override Task OnInitializedAsync()
    {
        //await ((ApiAuthenticationStateProvider)AuthenticationStateProvider).GetAuthenticationStateAsync();
    }
}
