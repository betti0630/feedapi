using Blazored.SessionStorage;

using System.Net.Http.Headers;

namespace AttrectoTest.BlazorWasm.Handlers;

internal class JwtAuthorizationMessageHandler : DelegatingHandler
{
    private readonly ISessionStorageService _sessionStorageService;

    public JwtAuthorizationMessageHandler(ISessionStorageService sessionStorageService)
    {
        _sessionStorageService = sessionStorageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_sessionStorageService);
        ArgumentNullException.ThrowIfNull(request);
        var token = await _sessionStorageService.GetItemAsync<string>("jwt", cancellationToken);
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
