namespace AttrectoTest.BlazorWasm.Services.IamBase;

public partial class AuthClient : IAuthClient
{
    public HttpClient HttpClient => _httpClient;
}
