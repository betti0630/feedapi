using AttrectoTest.Aim.Persistence.DatabaseContext;
using AttrectoTest.Aim.Application.Contracts.Identity;
using AttrectoTest.Aim.Domain;

using Microsoft.EntityFrameworkCore;


namespace AttrectoTest.Aim.Persistence.Seed;

internal class DbSeeder(TestDbContext dbContext, IAppUserService userService)
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
