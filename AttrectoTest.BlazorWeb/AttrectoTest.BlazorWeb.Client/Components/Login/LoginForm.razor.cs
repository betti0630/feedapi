using AttrectoTest.Blazor.Common.Contracts;

using Microsoft.AspNetCore.Components;

using System.Net.Http.Json;

namespace AttrectoTest.BlazorWeb.Client.Components.Login;

public partial class LoginForm
{


    [Inject] protected NavigationManager Nav { get; set; } = null!;
    [Inject] private HttpClient Http { get; set; } = default!;

    private string UserName = "";
    private string Password = "";
    private string? _error;

    private async Task LoginClick()
    {
        var result = await Http.PostAsJsonAsync("/api/auth/login", new { UserName, Password });
        if (result.IsSuccessStatusCode)
            Nav.NavigateTo("/");
        else
            _error = "Login failed";
    }

}
