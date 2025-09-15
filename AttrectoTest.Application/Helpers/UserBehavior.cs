using AttrectoTest.Application.Features.Base;
using AttrectoTest.Application.Features.Feed.Commands.CreateFeed;

using MediatR;

using Microsoft.AspNetCore.Http;

using System.Security.Claims;


namespace AttrectoTest.Application.Helpers;

public class UserIdBehavior<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is UserRequest req)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var userId = httpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int uid))
            {
                req.UserId = uid;
            }
            req.UserName = httpContext?.User?.FindFirstValue(ClaimTypes.Name);
        }

        return await next(cancellationToken);
    }
}
