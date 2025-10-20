namespace AttrectoTest.Iam.Application.Identity.Dtos;

public class UserDataDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string? RolesCsv { get; set; }
}
