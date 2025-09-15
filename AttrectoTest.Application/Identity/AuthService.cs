using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Domain;

using Microsoft.AspNetCore.Identity;

namespace AttrectoTest.Application.Identity;

internal class AuthService : IAuthService
{
    private readonly IAuthUserService _authUserService;
    private readonly IPasswordHasher<AppUser> _hasher;
    private readonly IGenericRepository<AppUser> _db;
    private readonly IAppLogger<AuthService> _logger;

    public AuthService(IAuthUserService authUserService,
        IPasswordHasher<AppUser> hasher, IGenericRepository<AppUser> db, 
        IAppLogger<AuthService> logger)
    {
        _authUserService = authUserService;
        _hasher = hasher;
        _db = db;
        _logger = logger;
    }

    public async Task<bool> ValidateUser(string userName, string password)
    {
        var user = await _db.GetByAsync(u => u.UserName == userName);
        if (user is null) { 
            _logger.LogWarning("User {UserName} not found during login attempt", userName);
            return false;
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed) { 
            _logger.LogWarning("Invalid password for user {UserName} during login attempt", userName);
            return false;
        }
        _logger.LogInformation("User {UserName} successfully authenticated", userName);
        return true;
    }

    public async Task<(string token, DateTime expires)> GenerateJwtToken(string userName)
    {
        var user = await _db.GetByAsync(u => u.UserName == userName);
        if (user is null)
        {
            _logger.LogWarning("User {UserName} not found when generating JWT token", userName);
            throw new NotFoundException(nameof(AppUser), userName);
        }

        return _authUserService.GenerateJwtToken(user);
    }

}
