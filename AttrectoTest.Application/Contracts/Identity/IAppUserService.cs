namespace AttrectoTest.Application.Contracts.Identity;

public interface IAppUserService
{
    Task AddNewUser(string userName, string password, string? roles);
}
