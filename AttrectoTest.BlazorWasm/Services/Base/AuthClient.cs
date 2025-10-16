namespace AttrectoTest.BlazorWasm.Services.Base;

public partial class AuthClient : IAuthClient
{
    public HttpClient HttpClient => _httpClient;
}
