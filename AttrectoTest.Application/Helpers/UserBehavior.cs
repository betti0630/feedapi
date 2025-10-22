using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Features.Base;

using MediatR;

using Microsoft.AspNetCore.Http;

using System.Security.Claims;


namespace AttrectoTest.Application.Helpers;

public class UserBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : UserRequest
{
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private readonly ICurrentUserService? _currentUserService;

    public UserBehavior(
        IHttpContextAccessor? httpContextAccessor = null,
        ICurrentUserService? currentUserService = null)
    {
        _httpContextAccessor = httpContextAccessor;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ClaimsPrincipal? user = null;

        if (_currentUserService != null)
        {
            user = _currentUserService.User;
        }

        if (user == null && _httpContextAccessor != null) {
            var httpContext = _httpContextAccessor.HttpContext;
            user = httpContext?.User;
        }
        var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userId, out int uid))
        {
            request.UserId = uid;
        }
        request.UserName = user?.FindFirstValue(ClaimTypes.Name);

        return await next(cancellationToken);
    }
}
