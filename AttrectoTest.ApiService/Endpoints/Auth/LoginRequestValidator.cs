using FastEndpoints;

using FluentValidation;

namespace AttrectoTest.ApiService.Endpoints.Auth;

public class LoginRequestValidator : Validator<LoginRequest>
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
