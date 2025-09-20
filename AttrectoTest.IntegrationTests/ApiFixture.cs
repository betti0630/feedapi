using Microsoft.Extensions.Configuration;

namespace AttrectoTest.IntegrationTests;

public class ApiFixture : IDisposable
{
    public HttpClient Client { get; }

    public ApiFixture()
    {
        var config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true)
           .AddEnvironmentVariables() 
           .Build();

        var baseUrl = config["ApiBaseUrl"] ?? "http://localhost:5001";
        Client = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}