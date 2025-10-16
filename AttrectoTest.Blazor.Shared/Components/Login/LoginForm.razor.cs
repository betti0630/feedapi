using AttrectoTest.Blazor.Shared.Contracts;

using Microsoft.AspNetCore.Components;

namespace AttrectoTest.Blazor.Shared.Components.Login;

public partial class LoginForm
{

    [Inject] protected IAuthService Auth { get; set; } = null!;
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
