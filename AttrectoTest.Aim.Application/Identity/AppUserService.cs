using AttrectoTest.Aim.Application.Contracts.Identity;
using AttrectoTest.Aim.Application.Contracts.Persistence;
using AttrectoTest.Aim.Application.Identity.Dtos;
using AttrectoTest.Aim.Domain;

using Microsoft.AspNetCore.Identity;

namespace AttrectoTest.Aim.Application.Identity;

internal class AppUserService(IPasswordHasher<AppUser> hasher, IGenericRepository<AppUser> db) : IAppUserService
{
    public async Task AddNewUser(string userName, string password, string? roles, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new InvalidOperationException("Username cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(password)) {
            throw new InvalidOperationException("Password cannot be empty.");
        }
        if (userName.Length < 3) {
            throw new InvalidOperationException("Username must be at least 3 characters long.");
        }

        if (await db.AnyAsync(u => u.UserName == userName, cancellationToken))
        {
            throw new InvalidOperationException($"User with username '{userName}' already exists.");
        }
        if (string.IsNullOrWhiteSpace(roles))
        {
            roles = "User";
        }
        if (string.IsNullOrWhiteSpace(password)) {
            throw new InvalidOperationException("Password cannot be empty.");
        }
        if (password.Length < 6) {
            throw new InvalidOperationException("Password must be at least 6 characters long.");
        }
        if (password.Length > 100) {
            throw new InvalidOperationException("Password cannot be longer than 100 characters.");
        }
        var user = new AppUser
        {
            UserName = userName,
            RolesCsv = roles
        };
        user.PasswordHash = hasher.HashPassword(user, password);
        await db.CreateAsync(user, cancellationToken);
    }

    public async Task<int> GetUserIdByUserName(string userName, CancellationToken cancellationToken = default)
    {
        var result = await db.GetByAsync(u => u.UserName.ToLower() == userName.Trim().ToLower(), cancellationToken);
        if (result == null) {
            throw new InvalidOperationException($"Not existing userName {userName}");
        }
        return result.Id;
    }

    public async Task<UserDataDto> GetUserData(int userId, CancellationToken cancellationToken = default)
    {
        var result = await db.GetByIdAsync(userId, cancellationToken);
        if (result == null)
        {
            throw new InvalidOperationException($"Not existing userId {userId}");
        }
        return new UserDataDto
        {
            Id = userId,
            UserName = result.UserName,
            RolesCsv = result.RolesCsv
        };
    }
}
