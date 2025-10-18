using AttrectoTest.Aim.Application.Contracts.Identity;
using AttrectoTest.Common.Grpc.Aim;

using Grpc.Core;

namespace AttrectoTest.AimService.Services;

public class AimGrpcService : AttrectoTest.Common.Grpc.Aim.AimService.AimServiceBase
{
    private readonly IAppUserService _appUserService;

    public AimGrpcService(IAppUserService appUserService)
    {
        _appUserService = appUserService;
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
