using AttrectoTest.Web.Providers;

using Blazored.LocalStorage;

using HR.LeaveManagement.BlazorUI.Providers;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

using System.Net.Http.Headers;

namespace AttrectoTest.Web.Auth;

public class TokenHandler : DelegatingHandler
{
    private readonly AuthenticationStateProvider _authProvider;

    public TokenHandler(AuthenticationStateProvider authProvider)
    {
        _authProvider = authProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        if (_authProvider is CustomAuthStateProvider custom)
        {
            var authState = await custom.GetAuthenticationStateAsync();
            //var token = custom.GetToken();
            //if (!string.IsNullOrWhiteSpace(token))
            //{
            //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //}
        }
        return await base.SendAsync(request, ct);
        
    }
}
