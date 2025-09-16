using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.ApiService.Dtos.Auth;

public record RegisterRequest
{
    public RegisterRequest(string @userName, string @password )
    {
        UserName = @userName;
        Password = @password;
    }

    [Required]
    [StringLength(int.MaxValue, MinimumLength = 3)]
    public string UserName { get; }

    [Required]
    [StringLength(int.MaxValue, MinimumLength = 6)]
    public string Password { get; }

}
