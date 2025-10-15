namespace AttrectoTest.Web.Services.Base;

public partial class AuthClient : IAuthClient
{
    public HttpClient HttpClient => _httpClient;
}
