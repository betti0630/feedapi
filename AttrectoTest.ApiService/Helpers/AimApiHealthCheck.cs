using AttrectoTest.ApiService.Configuration;
using AttrectoTest.Common.Grpc.Aim;

using Grpc.Net.Client;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace AttrectoTest.ApiService.Helpers;

public class AimApiHealthCheck : IHealthCheck
{
    private readonly ApiSettings _apiSettings;

    public AimApiHealthCheck(IOptions<ApiSettings> apiSettings)
    {
        _apiSettings = apiSettings.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var aimApiAddress = _apiSettings.AimBaseUrl;
        try
        {

            using var channel = GrpcChannel.ForAddress(aimApiAddress);
            var client = new AimService.AimServiceClient(channel);

            var reply = await client.PingAsync(new PingRequest(), cancellationToken: cancellationToken);


            return reply.Success
                ? HealthCheckResult.Healthy("Aim API reachable")
                : HealthCheckResult.Degraded("Aim API responded but not healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Aim API {aimApiAddress} not reachable", ex);
        }
    } 
}
