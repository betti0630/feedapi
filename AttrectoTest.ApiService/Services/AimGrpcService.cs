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

    public async Task<string> GetAimDataAsync(string id, CancellationToken ct = default)
    {
        var result = await _client.GetAimDataAsync(new AimRequest { Id = id }, cancellationToken: ct);
        return result.Name;
    }
}
