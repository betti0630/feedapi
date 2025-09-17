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
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User must be authenticated to update a feed.");

    }
}

public class UpdateVideoFeedCommandValidator : AbstractValidator<UpdateVideoFeedCommand>
{
    public UpdateVideoFeedCommandValidator()
    {
        Include(new UpdateFeedCommandValidator());

        RuleFor(x => x.VideoUrl)
            .Must(uri => uri is null || (Uri.TryCreate(uri, UriKind.Absolute, out var result)
                         && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps)))
            .WithMessage("Video must be a valid URL!");

    }
}
