namespace AttrectoTest.Domain;

public class AppUser
{
    public int Id { get; set; }
    public string UserName { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string? RolesCsv { get; set; }
}
