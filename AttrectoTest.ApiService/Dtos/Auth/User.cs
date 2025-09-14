using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.ApiService.Dtos.Auth
{
    public partial class User
    {
        public User(DateTimeOffset @createdAt, string @email, Guid @id, string @userName)
        {
            Id = @id;
            UserName = @userName;
            Email = @email;
            CreatedAt = @createdAt;
        }

        [Required(AllowEmptyStrings = true)]
        public Guid Id { get; }

        [Required(AllowEmptyStrings = true)]
        public string UserName { get; }

        [Required(AllowEmptyStrings = true)]
        public string Email { get; }

        [Required(AllowEmptyStrings = true)]
        public DateTimeOffset CreatedAt { get; }

    }


}
