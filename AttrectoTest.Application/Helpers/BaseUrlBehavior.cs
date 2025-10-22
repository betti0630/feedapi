using AttrectoTest.Application.Contracts.Requests;

using MediatR;

using Microsoft.AspNetCore.Http;


namespace AttrectoTest.Application.Helpers;

public class BaseUrlBehavior<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseUrlAwareRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(next);

        var httpRequest = httpContextAccessor.HttpContext?.Request;
        if (httpRequest != null)
        {
            var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}";
            request.BaseUrl = new Uri(baseUrl);
        }
        return await next(cancellationToken).ConfigureAwait(false);
    }
}
