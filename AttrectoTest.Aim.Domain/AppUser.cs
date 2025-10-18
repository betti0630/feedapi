using AttrectoTest.Aim.Domain.Common;

using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.Aim.Domain;

public class AppUser : AppEntity
{
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = default!;
    [Required]
    public string PasswordHash { get; set; } = default!;
    public string? RolesCsv { get; set; }
}
