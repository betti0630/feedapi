using AttrectoTest.Common.Grpc.Iam;
using AttrectoTest.Iam.Application.Contracts.Identity;

using Grpc.Core;


namespace AttrectoTest.IamService.Services;

internal class IamGrpcService : AttrectoTest.Common.Grpc.Iam.IamService.IamServiceBase
{
    private readonly IAppUserService _appUserService;

    public IamGrpcService(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }

    public override Task<PingResponse> Ping(PingRequest request, ServerCallContext context)
    {
        return Task.FromResult(new PingResponse { Success = true });
    }

    public override async Task<GetUserIdByUserNameResponse> GetUserIdByUserName(GetUserIdByUserNameRequest request, ServerCallContext context)
    {
        var userId = await _appUserService.GetUserIdByUserName(request.UserName);
        return new GetUserIdByUserNameResponse
        {
            UserId = userId,
        };
    }

    public override async Task<GetUserDataResponse> GetUserData(GetUserDataRequest request, ServerCallContext context)
    {
        var userData = await _appUserService.GetUserData(request.UserId);
        return new GetUserDataResponse
        {
            UserId = userData.Id,
            UserName = userData.UserName,
            Roles = userData.RolesCsv,
        };
    }

}
