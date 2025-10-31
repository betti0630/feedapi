using FeedApp.Blazor.Common.Contracts;

using FeedApp.BlazorServer.Providers;

using Microsoft.AspNetCore.Components.Authorization;
using AttrectoTest.BlazorServer.Services.IamBase;

namespace FeedApp.BlazorServer.Services;

internal class AuthManager : IAuthManager
{
    private readonly IAuthClient _authClient;
    private readonly AuthenticationStateProvider _authProvider;


    public AuthManager(IAuthClient authClient, AuthenticationStateProvider authProvider)
    {

        _authClient = authClient;
        _authProvider = authProvider;
    }

    public async Task<bool> Login(string userName, string password)
    {
        var request = new LoginRequest { UserName = userName, Password = password };
        var response = await _authClient.LoginAsync(request);

        if (string.IsNullOrEmpty(response.AccessToken))
        {
            return false;
        }

        var jwtToken = response.AccessToken;
        await ((CustomAuthStateProvider)_authProvider).MarkUserAsAuthenticated(jwtToken);
        return true;
    }

    public async Task Logout()
    {
        await ((CustomAuthStateProvider)_authProvider).MarkUserAsLoggedOut();

    }
}
