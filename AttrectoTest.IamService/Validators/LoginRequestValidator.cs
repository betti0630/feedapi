
using FluentValidation;
using AttrectoTest.Iam.Application.Identity.Dtos.Auth;

namespace AttrectoTest.IamService.Validators;

internal class LoginRequestValidator : AbstractValidator<LoginRequest>
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

