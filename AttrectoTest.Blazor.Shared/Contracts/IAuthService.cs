namespace AttrectoTest.Blazor.Shared.Contracts;

public interface IAuthService
{
    Task<bool> Login(string userName, string password);
    Task Logout();
}
