using AttrectoTest.ApiService.Controllers;
using AttrectoTest.ApiService.Dtos.Auth;

using FluentValidation;

namespace AttrectoTest.ApiService.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required!")
            .MinimumLength(3).WithMessage("Username minimum length is 3!");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required!")
            .MinimumLength(3).WithMessage("Password minimum length is 3");
    }
}

