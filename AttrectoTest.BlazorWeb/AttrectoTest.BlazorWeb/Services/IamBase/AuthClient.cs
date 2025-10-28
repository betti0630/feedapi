namespace AttrectoTest.BlazorServer.Services.IamBase;

public partial class AuthClient : IAuthClient
{
    public HttpClient HttpClient => _httpClient;
}
