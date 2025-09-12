using AttrectoTest.Web.Auth;
using AttrectoTest.Web.Providers;

using Blazored.LocalStorage;

using HR.LeaveManagement.BlazorUI.Providers;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace AttrectoTest.Web.Components.Pages;

public partial class Login
{
    [Inject]
    private IHttpClientFactory Clients { get; set; } = null!;


    [Inject]
    private AuthenticationStateProvider AuthProvider { get; set; } = null!;

    string user = "test", pass = "Passw0rd!";
    string? error;


    async void OnClickLogin()
    {
        var client = Clients.CreateClient("api");
        var resp = await client.PostAsJsonAsync("/auth/login", new { userName = user, password = pass });
        if (!resp.IsSuccessStatusCode) { error = "Hibás bejelentkezés"; return; }

        var data = await resp.Content.ReadFromJsonAsync<LoginDto>();
        //var customAuth = (AppAuthStateProvider)AuthProvider;
        //await ((ApiAuthenticationStateProvider)AuthProvider).LoggedIn();
        var provider = (CustomAuthStateProvider)AuthProvider;
        await provider.LoginAsync(user, data!.Token);
        //await customAuth.MarkUserAsAuthenticated(data!.Token);
        error = null;
    }

    record LoginDto(string Token, DateTime ExpiresAt);
}
