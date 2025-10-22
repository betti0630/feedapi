using Blazored.SessionStorage;

using Microsoft.AspNetCore.Components.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AttrectoTest.BlazorServer.Providers;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorage;
    private const string TOKEN_KEY = "authToken";
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    private string? _token;
    private bool _initialized = false;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public CustomAuthStateProvider(ISessionStorageService sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!string.IsNullOrWhiteSpace(_token))
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jwt = handler.ReadJwtToken(_token);
                var identity = new ClaimsIdentity(jwt.Claims, "jwt");
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
            }
            catch
            {
                return Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        return Task.FromResult(new AuthenticationState(_anonymous));
    }

    public async Task InitializeAsync()
    {
        if (_initialized)
            return;

        await _initLock.WaitAsync();
        try
        {
            if (_initialized)
                return;

            try
            {
                var token = await _sessionStorage.GetItemAsync<string>(TOKEN_KEY);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(token);
                    _token = token;

                    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(
                        new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims, "jwt"))
                    )));
                }
            }
            catch (InvalidOperationException)
            {
            }
            catch
            {
                try { await _sessionStorage.RemoveItemAsync(TOKEN_KEY); } catch { }
            }

            _initialized = true;
        }
        finally
        {
            _initLock.Release();
        }
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        _token = token;
        try
        {
            await _sessionStorage.SetItemAsync(TOKEN_KEY, token);
        }
        catch (InvalidOperationException)
        {
        }

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims, "jwt"))
        )));
    }

    public async Task MarkUserAsLoggedOut()
    {
        _token = null;
        try
        {
            await _sessionStorage.RemoveItemAsync(TOKEN_KEY);
        }
        catch (InvalidOperationException)
        {
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }
}