using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;

using FluentValidation;

namespace AttrectoTest.Application.Features.Feed.Commands.CreateFeed;

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
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User must be authenticated to create a feed.");
    }
}

public class CreateVideoFeedCommandValidator : AbstractValidator<CreateVideoFeedCommand>
{
    public CreateVideoFeedCommandValidator()
    {
        Include(new CreateFeedCommandValidator());

        RuleFor(x => x.VideoUrl)
            .NotEmpty().WithMessage("Video URL is required!")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out var result)
                         && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps))
            .WithMessage("Video must be a valid URL!");

    }
}
