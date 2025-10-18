using AttrectoTest.Aim.Application.Contracts.Requests;

using MediatR;

using Microsoft.AspNetCore.Http;


namespace AttrectoTest.Aim.Application.Helpers;

public class BaseUrlBehavior<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseUrlAwareRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var httpRequest = httpContextAccessor.HttpContext?.Request;
        if (httpRequest != null)
        {
            var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}";
            request.BaseUrl = baseUrl;
        }
        return await next(cancellationToken);
    }
}
