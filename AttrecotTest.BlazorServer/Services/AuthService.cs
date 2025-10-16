using AttrectoTest.Blazor.Shared.Contracts;

namespace AttrecotTest.BlazorServer.Services;

public class AuthService : IAuthService
{
    public Task<bool> Login(string userName, string password)
    {
        throw new NotImplementedException();
    }

    public Task Logout()
    {
        throw new NotImplementedException();
    }
}
