using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Common.Grpc.Aim;

using Grpc.Net.Client;

namespace AttrectoTest.ApiService.Services;

public class AimGrpcService : IAimService
{
    private readonly AimService.AimServiceClient _client;

    public AimGrpcService(string aimApiUrl)
    {
        var channel = GrpcChannel.ForAddress(aimApiUrl);
        _client = new AimService.AimServiceClient(channel);
    }

    public async Task<int> GetUserIdByUserName(string userName, CancellationToken ct = default)
    {
        var result = await _client.GetUserIdByUserNameAsync(new GetUserIdByUserNameRequest { UserName = userName }, cancellationToken: ct);
        return result.UserId;
    }

    public async Task<string> GetUserNameByUserId(int userId, CancellationToken ct = default)
    {
        var result = await _client.GetUserDataAsync(new GetUserDataRequest { UserId = userId }, cancellationToken: ct);
        return result.UserName;
    }
}
