using AttrectoTest.Aim.Application.Identity.Dtos;

namespace AttrectoTest.Aim.Application.Contracts.Identity;

public interface IAppUserService
{
    Task AddNewUser(string userName, string password, string? roles, CancellationToken cancellationToken = default);
    Task<int> GetUserIdByUserName(string userName, CancellationToken cancellationToken = default);
    Task<UserDataDto> GetUserData(int userId, CancellationToken cancellationToken = default);
}
