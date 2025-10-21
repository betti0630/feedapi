using AttrectoTest.Application.Contracts.Identity;

namespace AttrectoTest.BlazorServer.Services;

public class IamGrpcService : IIamService
{
    public IamGrpcService(string iamApiUrl)
    {

    }

    public async Task<int> GetUserIdByUserName(string userName, CancellationToken ct = default)
    {
        return 1;
    }

    public async Task<string> GetUserNameByUserId(int userId, CancellationToken ct = default)
    {
        return "alice";
    }
}
