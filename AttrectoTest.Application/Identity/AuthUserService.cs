using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Models;
using AttrectoTest.Domain;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace AttrectoTest.Application.Identity;

internal class AuthUserService( IHttpContextAccessor httpContextAccessor) : IAuthUserService
{


    public int? UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int uid)) {
                return uid;
            }
            return null;
        }
    }
    public string? UserName => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

}
