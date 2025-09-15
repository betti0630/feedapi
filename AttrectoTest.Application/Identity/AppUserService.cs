using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;

using Microsoft.AspNetCore.Identity;

namespace AttrectoTest.Application.Identity;

internal class AppUserService(IPasswordHasher<AppUser> hasher, IGenericRepository<AppUser> db) : IAppUserService
{
    public async Task AddNewUser(string userName, string password, string? roles)
    {

        var user = new AppUser
        {
            UserName = userName,
            RolesCsv = roles
        };
        user.PasswordHash = hasher.HashPassword(user, password);
        await db.CreateAsync(user);
    }
}
