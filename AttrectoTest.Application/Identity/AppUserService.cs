using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Domain;

using Microsoft.AspNetCore.Identity;

using static System.Formats.Asn1.AsnWriter;

namespace AttrectoTest.Application.Identity;

internal class AppUserService : IAppUserService
{
    private readonly IPasswordHasher<AppUser> _hasher;
    private readonly IGenericRepository<AppUser> _db;

    public AppUserService(IPasswordHasher<AppUser> hasher, IGenericRepository<AppUser> db)
    {
        _hasher = hasher;
        _db = db;
    }

    public async Task AddNewUser(string userName, string password, string? roles)
    {
        if (!await _db.AnyAsync())
        {
            var user = new AppUser
            {
                UserName = userName,
                RolesCsv = roles
            };
            user.PasswordHash = _hasher.HashPassword(user, password);
            await _db.CreateAsync(user);
        }
    }
}
