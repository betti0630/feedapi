using AttrectoTest.ApiService.Configuration;
using AttrectoTest.Common.Grpc.Iam;

using Grpc.Net.Client;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

using static AttrectoTest.Common.Grpc.Iam.IamService;

namespace AttrectoTest.ApiService.Helpers;

internal class IamApiHealthCheck : IHealthCheck
{
    private readonly ApiSettings _apiSettings;

    public IamApiHealthCheck(IOptions<ApiSettings> apiSettings)
    {
        _apiSettings = apiSettings.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var iamApiAddress = _apiSettings.IamBaseUrl;
        try
        {

            using var channel = GrpcChannel.ForAddress(iamApiAddress);
            var client = new IamServiceClient(channel);

            var reply = await client.PingAsync(new PingRequest(), cancellationToken: cancellationToken);


            return reply.Success
                ? HealthCheckResult.Healthy("Iam API reachable")
                : HealthCheckResult.Degraded("Iam API responded but not healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Iam API {iamApiAddress} not reachable", ex);
        }
    } 
}
