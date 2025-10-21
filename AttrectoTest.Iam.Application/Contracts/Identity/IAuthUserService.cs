
using AttrectoTest.Iam.Domain;

namespace AttrectoTest.Iam.Application.Contracts.Identity;

public interface IAuthUserService
{
    public int? UserId { get; }
    public string? UserName { get; }
    (string token, DateTime expires) GenerateJwtToken(AppUser user);
}
