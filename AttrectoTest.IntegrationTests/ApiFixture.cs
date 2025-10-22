using AttrectoTest.Iam.Application.Identity.Dtos.Auth;

using Microsoft.Extensions.Configuration;

using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AttrectoTest.IntegrationTests;

public sealed class ApiFixture : IDisposable
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

    public async Task<HttpClient> GetAuthenticatedClient()
    {
        var loginRequest = new { Username = "alice", Password = "Passw0rd!" };

        var response = await Client.PostAsJsonAsync("/api/auth/login", loginRequest);
        response.EnsureSuccessStatusCode();

        var token = (await response.Content.ReadFromJsonAsync<LoginResponse>())?.Access_token;
        Assert.False(string.IsNullOrEmpty(token));

        var authClient = new HttpClient { BaseAddress = Client.BaseAddress };
        authClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        return authClient;
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}