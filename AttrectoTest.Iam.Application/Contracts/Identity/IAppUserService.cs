using AttrectoTest.Iam.Application.Identity.Dtos;

namespace AttrectoTest.Iam.Application.Contracts.Identity;

public interface IAppUserService
{
    Task AddNewUser(string userName, string password, string firstName, string lastName, string email, string? roles, CancellationToken cancellationToken = default);
    Task<int> GetUserIdByUserName(string userName, CancellationToken cancellationToken = default);
    Task<UserDataDto> GetUserData(int userId, CancellationToken cancellationToken = default);
}
