using AttrectoTest.Domain.Common;

namespace AttrectoTest.Domain;

public class AppUser : AppEntity
{
    public string UserName { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string? RolesCsv { get; set; }
}
