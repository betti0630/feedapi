using AttrectoTest.Blazor.Common.Contracts;

using Microsoft.AspNetCore.Components;

namespace AttrectoTest.Blazor.Common.Components.Login;

public partial class LoginForm
{

    [Inject] protected IAuthManager Auth { get; set; } = null!;
    [Inject] protected NavigationManager Nav { get; set; } = null!;

    private string _userName = "";
    private string _password = "";
    private string? _error;

    private async Task LoginClick()
    {
        var ok = await Auth.Login(_userName, _password);
        if (ok)
        {
            Nav.NavigateTo("/");
        }
        else
        {
            _error = "Login failed";
        }
    }
}
