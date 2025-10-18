using AttrectoTest.Common.Grpc.Aim;

using Grpc.Core;

namespace AttrectoTest.AimService.Services;

public class AimGrpcService : AttrectoTest.Common.Grpc.Aim.AimService.AimServiceBase
{
    public override Task<AimResponse> GetAimData(AimRequest request, ServerCallContext context)
    {
        return Task.FromResult(new AimResponse
        {
            Name = "TestUser",
            Score = 42.5
        });
    }
}
