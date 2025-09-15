using FluentValidation;

namespace AttrectoTest.Application.Features.Feed.Commands.DeleteFeed;

public class DeleteFeedCommandValidator : AbstractValidator<DeleteFeedCommand>
{
    public DeleteFeedCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0!");
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User must be authenticated to delete a feed.");
    }
}
