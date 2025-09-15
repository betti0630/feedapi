using FluentValidation;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;

public class UpdateFeedCommandValidator : AbstractValidator<UpdateFeedCommand>
{
    public UpdateFeedCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0!");
        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Title maximum length is 100 characters!");
        RuleFor(x => x.Content)
            .MaximumLength(5000).WithMessage("Content maximum length is 5000 characters!");
    }
}
