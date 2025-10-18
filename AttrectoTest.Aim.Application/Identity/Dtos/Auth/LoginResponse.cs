using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.Aim.Application.Identity.Dtos.Auth
{
    public record LoginResponse
    {
        public LoginResponse(string @access_token)
        {
            Access_token = @access_token;
        }

        [Required(AllowEmptyStrings = true)]
        public string Access_token { get; set; }

    }


}
