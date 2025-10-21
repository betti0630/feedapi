using AttrectoTest.Application.Exceptions;
using AttrectoTest.Iam.Application.Contracts.Identity;
using AttrectoTest.Iam.Application.Contracts.Logging;
using AttrectoTest.Iam.Application.Contracts.Persistence;
using AttrectoTest.Iam.Domain;

using Microsoft.AspNetCore.Identity;

namespace AttrectoTest.Iam.Application.Identity;

internal class AuthService(IAuthUserService authUserService,
    IPasswordHasher<AppUser> hasher, IGenericRepository<AppUser> db,
    IAppLogger<AuthService> logger) : IAuthService
{
    public async Task<bool> ValidateUser(string userName, string password, CancellationToken cancellationToken = default)
    {
        var user = await db.GetByAsync(u => u.UserName == userName, cancellationToken);
        if (user is null) { 
            logger.LogWarning("User {UserName} not found during login attempt", userName);
            return false;
        }

        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed) { 
            logger.LogWarning("Invalid password for user {UserName} during login attempt", userName);
            return false;
        }
        logger.LogInformation("User {UserName} successfully authenticated", userName);
        return true;
    }

    public async Task<(string token, DateTime expires)> GenerateJwtToken(string userName)
    {
        var user = await db.GetByAsync(u => u.UserName == userName);
        if (user is null)
        {
            logger.LogWarning("User {UserName} not found when generating JWT token", userName);
            throw new NotFoundException(nameof(AppUser), userName);
        }

        return authUserService.GenerateJwtToken(user);
    }

}
