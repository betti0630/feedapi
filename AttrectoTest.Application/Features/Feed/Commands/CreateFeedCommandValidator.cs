using FluentValidation;

namespace AttrectoTest.Application.Features.Feed.Commands;

public class CreateFeedCommandValidator : AbstractValidator<CreateFeedCommand>
{
    public CreateFeedCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required!")
            .MaximumLength(100).WithMessage("Title maximum length is 100 characters!");
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required!")
            .MaximumLength(5000).WithMessage("Content maximum length is 5000 characters!");
    }
}
