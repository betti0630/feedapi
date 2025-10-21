using AttrectoTest.BlazorServer.Helpers;

using Blazored.SessionStorage;

using Microsoft.AspNetCore.Components.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AttrectoTest.BlazorServer.Providers;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorage;
    private const string TOKEN_KEY = "authToken";
    private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    // memóriában tárolt token, hogy GetAuthenticationStateAsync gyors legyen
    private string? _token;
    private bool _initialized = false;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public CustomAuthStateProvider(ISessionStorageService sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    // Ezt hívják a Blazor, pl. AuthorizeView stb.
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Ha van már memóriában token (és sikeresen feldolgoztuk),
        // akkor azonnal visszaadjuk a megfelelő ClaimsPrincipal-t.
        if (!string.IsNullOrWhiteSpace(_token))
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jwt = handler.ReadJwtToken(_token);
                var identity = new ClaimsIdentity(jwt.Claims, "jwt");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch
            {
                // hibás token esetén fallback anonim
                return new AuthenticationState(_anonymous);
            }
        }

        // Ha még nem inicializáltunk kliensoldalon, akkor visszaadunk anonim állapotot.
        // (Ne próbáljunk itt JS-t hívni; ha prerender fázisban vagyunk, az dobna.)
        return new AuthenticationState(_anonymous);
    }

    // Ezt hívjuk kliensoldalon (OnAfterRenderAsync első futásakor), hogy ténylegesen próbáljuk meg beolvasni a token-t.
    public async Task InitializeAsync()
    {
        // Ha már inicializáltunk (vagy éppen inicializálás folyamatban), ne csináljunk újat.
        if (_initialized)
            return;

        await _initLock.WaitAsync();
        try
        {
            if (_initialized)
                return;

            try
            {
                // Ez a hívás JS interop-ot indít; prerender alatt InvalidOperationException-t fog dobni.
                var token = await _sessionStorage.GetItemAsync<string>(TOKEN_KEY);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    // érdemes itt validálni/parsolni, hogy ne tároljunk hibás tokent
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(token); // ha hibás, kivétel dobódik
                    _token = token;

                    // jelentsük a rendszernek, hogy változott az auth állapot
                    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(
                        new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims, "jwt"))
                    )));
                }
            }
            catch (InvalidOperationException)
            {
                // JS runtime még nem elérhető (prerender fázis) — nem baj, csak ne próbáljuk tovább itt.
            }
            catch
            {
                // bármilyen más hiba esetén próbáljuk meg törölni a hibás tokent a storage-ból,
                // de ezt is csak ha a JS runtime elérhető (ez a try-ban van).
                try { await _sessionStorage.RemoveItemAsync(TOKEN_KEY); } catch { }
            }

            _initialized = true;
        }
        finally
        {
            _initLock.Release();
        }
    }

    // Login után ezt hívd, hogy beállítsa a tokent és elmentse a sessionStorage-ba
    public async Task MarkUserAsAuthenticated(string token)
    {
        _token = token;
        try
        {
            await _sessionStorage.SetItemAsync(TOKEN_KEY, token);
        }
        catch (InvalidOperationException)
        {
            // nagyon ritka, de ha mégis prerender alatt hívnánk, akkor a storage write nem megy — de _token memóriában megvan
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
            // prerender alatt nem írunk, de _token már null
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }
}