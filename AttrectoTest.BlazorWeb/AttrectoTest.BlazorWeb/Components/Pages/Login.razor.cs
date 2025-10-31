using AttrectoTest.Blazor.Common.Contracts;

using Microsoft.AspNetCore.Components;

namespace AttrectoTest.BlazorWeb.Components.Pages;

public partial class Login
{

    [Inject] protected IAuthManager Auth { get; set; } = null!;
    [Inject] protected NavigationManager Nav { get; set; } = null!;



    private async Task DoLogin((string userName, string password) p)
    {
        var ok = await Auth.Login(p.userName, p.password);
    }
}
