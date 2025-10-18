namespace AttrectoTest.BlazorWasm.Services.AimBase;

public partial class AuthClient : IAuthClient
{
    public HttpClient HttpClient => _httpClient;
}
