using FluentValidation;

namespace AttrectoTest.Application.Features.Feed.Commands.CreateFeed;

public class DeleteFeedCommandValidator : AbstractValidator<DeleteFeedCommand>
{
    public DeleteFeedCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0!");
    }
}
