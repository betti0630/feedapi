using AttrectoTest.Domain;

namespace AttrectoTest.Application.Contracts.Identity;

public interface IAuthService
{
    public Task<bool> ValidateUser(string userName, string password);
    public Task<(string token, DateTime expires)> GenerateJwtToken(string userName);
}
