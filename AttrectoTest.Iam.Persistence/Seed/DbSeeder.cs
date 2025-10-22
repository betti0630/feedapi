
using Microsoft.EntityFrameworkCore;
using AttrectoTest.Iam.Persistence.DatabaseContext;
using AttrectoTest.Iam.Application.Contracts.Identity;

namespace AttrectoTest.Iam.Persistence.Seed;

internal sealed class DbSeeder(AuthDbContext dbContext, IAppUserService userService) : IDbSeeder
{
    public async Task SeedAsync()
    {
        if (await dbContext.AppUsers.AnyAsync())
        {
            return; // DB has been seeded
        }
        var users = new List<(string UserName, string Password, string Role)>
        {
            ("alice", "Passw0rd!", "User"),
            ("bob", "Passw0rd!", "User"),
            ("admin", "AdminPassw0rd!", "Admin")
        };
        
        foreach (var (userName, password, role) in users)
        {
            await userService.AddNewUser(userName, password, role);
        }
        var Alice = await dbContext.AppUsers.FirstAsync(u => u.UserName == "alice");
        var Bob = await dbContext.AppUsers.FirstAsync(u => u.UserName == "bob");


  
        await dbContext.SaveChangesAsync();
    }


}
