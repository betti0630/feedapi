using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;

using Microsoft.AspNetCore.Identity;

namespace AttrectoTest.Application.Identity;

internal class AppUserService(IPasswordHasher<AppUser> hasher, IGenericRepository<AppUser> db) : IAppUserService
{
    public async Task AddNewUser(string userName, string password, string? roles, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new Exception("Username cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(password)) {
            throw new Exception("Password cannot be empty.");
        }
        if (userName.Length < 3) {
            throw new Exception("Username must be at least 3 characters long.");
        }

        if (await db.AnyAsync(u => u.UserName == userName, cancellationToken))
        {
            throw new Exception($"User with username '{userName}' already exists.");
        }
        if (string.IsNullOrWhiteSpace(roles))
        {
            roles = "User";
        }
        if (string.IsNullOrWhiteSpace(password)) {
            throw new Exception("Password cannot be empty.");
        }
        if (password.Length < 6) {
            throw new Exception("Password must be at least 6 characters long.");
        }
        if (password.Length > 100) {
            throw new Exception("Password cannot be longer than 100 characters.");
        }
        var user = new AppUser
        {
            UserName = userName,
            RolesCsv = roles
        };
        user.PasswordHash = hasher.HashPassword(user, password);
        await db.CreateAsync(user, cancellationToken);
    }
}
