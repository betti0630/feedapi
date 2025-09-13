using AttrectoTest.Application.Exceptions;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttrectoTest.Application.Features.Feed.Commands;

internal class CreateFeedCommandHandler : IRequestHandler<CreateFeedCommand, int>
{
    public async Task<int> Handle(CreateFeedCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateFeedCommandValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any()) { 
            throw new BadRequestException("Invalid feed", validationResult);
        }
        return 0;
    }
}
