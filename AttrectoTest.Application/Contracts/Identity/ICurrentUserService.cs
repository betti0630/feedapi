using System.Security.Claims;

namespace AttrectoTest.Application.Contracts.Identity;

public interface ICurrentUserService
{
    ClaimsPrincipal? User { get; }
}
