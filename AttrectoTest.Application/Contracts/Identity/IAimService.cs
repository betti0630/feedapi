
namespace AttrectoTest.Application.Contracts.Identity;

public interface IAimService
{
    Task<int> GetUserIdByUserName(string userName, CancellationToken ct = default);
    Task<string> GetUserNameByUserId(int userId, CancellationToken ct = default);
}
