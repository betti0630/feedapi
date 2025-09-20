using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Identity.Dtos.Auth;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AttrectoTest.IntegrationTests;

public class FeedApiTests(ApiFixture fixture) : IClassFixture<ApiFixture>
{
    private readonly HttpClient _client = fixture.Client;

    [Fact]
    public async Task GetFeeds_WithoutToken_ShouldReturn401()
    {
        // Act
        var response = await _client.GetAsync("/api/feeds");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetFeeds_WithValidToken_ShouldReturn200()
    {
        var loginRequest = new
        {
            Username = "alice",
            Password = "Passw0rd!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        var tokenResponse = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(tokenResponse);
        Assert.False(string.IsNullOrEmpty(tokenResponse.Access_token));

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenResponse.Access_token);

        // Act
        var response = await _client.GetAsync("/api/feeds");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var feeds = await response.Content.ReadFromJsonAsync<PagedFeeds> ();
        Assert.NotNull(feeds);
        Assert.True(feeds.Items.Count > 0);
    }
}

