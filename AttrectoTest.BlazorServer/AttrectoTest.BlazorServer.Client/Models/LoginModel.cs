using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.BlazorServer.Client.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Username is Required")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Input Invalid")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is Required")]
    public string Password { get; set; }
}
