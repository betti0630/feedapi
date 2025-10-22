using AttrectoTest.Blazor.Common.Contracts;
using AttrectoTest.BlazorWasm.Providers;
using AttrectoTest.BlazorWasm.Services.IamBase;

using Blazored.SessionStorage;

using Microsoft.AspNetCore.Components.Authorization;

namespace AttrectoTest.BlazorWasm.Services;

internal class AuthManager : IAuthManager
{
    private readonly IAuthClient _authClient;
    private readonly ISessionStorageService _sessionStorage;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILogger<AuthManager> _logger;


    public AuthManager(IAuthClient authClient, ISessionStorageService sessionStorage, AuthenticationStateProvider authStateProvider,
        ILogger<AuthManager> logger)
    {
        _authClient = authClient;
        _sessionStorage = sessionStorage;
        _authStateProvider = authStateProvider;
        _logger = logger;
    }

    public async Task<bool> Login(string userName, string password)
    {
        try
        {
            var request = new LoginRequest { UserName = userName, Password = password };
            var response = await _authClient.LoginAsync(request);

            if (string.IsNullOrEmpty(response.AccessToken))
            {
                return false;
            }

            var jwtToken = response.AccessToken;
            await _sessionStorage.SetItemAsync("jwt", jwtToken);
            ((JwtAuthStateProvider)_authStateProvider).NotifyUserAuthentication(jwtToken);
            return true;
        }
        #pragma warning disable CA1031
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
        #pragma warning restore CA1031
    }

    public async Task Logout()
    {
        await ((JwtAuthStateProvider)_authStateProvider).NotifyUserLogout();
    }
}
