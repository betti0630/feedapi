using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.ApiService.Dtos.Auth;

public record RegisterRequest
{
    public RegisterRequest(string @email, string @password, string @userName)
    {
        UserName = @userName;
        Email = @email;
        Password = @password;
    }

    [Required]
    [StringLength(int.MaxValue, MinimumLength = 3)]
    public string UserName { get; }

    [Required(AllowEmptyStrings = true)]
    public string Email { get; }

    [Required]
    [StringLength(int.MaxValue, MinimumLength = 6)]
    public string Password { get; }

}
