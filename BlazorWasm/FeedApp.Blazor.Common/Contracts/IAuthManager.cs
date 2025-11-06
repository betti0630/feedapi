namespace FeedApp.Blazor.Common.Contracts;

public interface IAuthManager
{
    Task<bool> Login(string userName, string password);
    Task Logout();
}
