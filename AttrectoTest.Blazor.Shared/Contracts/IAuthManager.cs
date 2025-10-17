namespace AttrectoTest.Blazor.Shared.Contracts;

public interface IAuthManager
{
    Task<bool> Login(string userName, string password);
    Task Logout();
}
