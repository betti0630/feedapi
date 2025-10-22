using AttrectoTest.Application.Features.Feed.Commands.CreateFeed;
using AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;
using AttrectoTest.Application.Features.Feed.Dtos;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AttrectoTest.IntegrationTests;

public class FeedApiTests(ApiFixture fixture) : IClassFixture<ApiFixture>
{

    [Fact]
    public async Task GetFeeds_WithoutToken_ShouldReturn401()
    {
        // Act
        var response = await fixture.Client.GetAsync(new Uri("/api/feeds", UriKind.Relative));

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetFeeds_WithValidToken_ShouldReturn200()
    {

        using var client = await fixture.GetAuthenticatedClient();

        // Act
        var response = await client.GetAsync(new Uri("/api/feeds", UriKind.Relative));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var feeds = await response.Content.ReadFromJsonAsync<PagedFeeds> ();
        Assert.NotNull(feeds);
        Assert.True(feeds.Items.Count > 0);
    }

    [Fact]
    public async Task PostFeed_ShouldReturnCreatedId()
    {
        using var client = await fixture.GetAuthenticatedClient();

        var newFeed = new CreateFeedCommand() { Title= "Test feed", Content = "Hello from test" };

        var response = await client.PostAsJsonAsync("/api/feeds", newFeed);

        response.EnsureSuccessStatusCode(); 
        var created = await response.Content.ReadFromJsonAsync<CreateFeedCommandResponse>();

        Assert.NotNull(created);
        Assert.Equal(newFeed.Title, created.Title);
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task GetFeed_ValidId_ShouldReturn200()
    {
        using var client = await fixture.GetAuthenticatedClient();

        // először létrehozunk egy feedet
        var create = new CreateFeedCommand() { Title = "Title1", Content = "Content1" } ;
        var created = await (await client.PostAsJsonAsync("/api/feeds", create))
                        .Content.ReadFromJsonAsync<CreateFeedCommandResponse>();

        Assert.NotNull(created);
        var response = await client.GetAsync(new Uri($"/api/feeds/{created.Id}", UriKind.Relative));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var fetched = await response.Content.ReadFromJsonAsync<FeedDto>();
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched.Id);
    }

    [Fact]
    public async Task GetFeed_InvalidId_ShouldReturn404()
    {
        using var client = await fixture.GetAuthenticatedClient();

        var response = await client.GetAsync(new Uri("/api/feeds/999999", UriKind.Relative));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PatchFeed_ValidUpdate_ShouldReturn200()
    {
        using var client = await fixture.GetAuthenticatedClient();

        var created = await (await client.PostAsJsonAsync("/api/feeds",
            new CreateFeedCommand() {Title = "Old title", Content = "Old content" }))
            .Content.ReadFromJsonAsync<FeedDto>();
        Assert.NotNull(created);
        
        var update = new UpdateFeedCommand() { Id = created.Id, Title = "New title", Content = "New content" };

        var response = await client.PatchAsJsonAsync($"/api/feeds/{created.Id}", update);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updated = await (await client.GetAsync(new Uri($"/api/feeds/{created.Id}", UriKind.Relative)))
            .Content.ReadFromJsonAsync<UpdateFeedCommandResponse>();

        Assert.NotNull(updated);
        Assert.Equal("New title", updated.Title);
    }

    [Fact]
    public async Task PatchFeed_InvalidUpdate_ShouldReturn400()
    {
        using var client = await fixture.GetAuthenticatedClient();

        var created = await (await client.PostAsJsonAsync("/api/feeds",
            new CreateFeedCommand() { Title = "Good title", Content = "Good content" }))
            .Content.ReadFromJsonAsync<CreateFeedCommandResponse>();

        Assert.NotNull(created);

        var longtitle = new string('A', 101);
        var update = new UpdateFeedCommand() { Id = created.Id, Title = longtitle, Content = "Still content" };

        var response = await client.PatchAsJsonAsync($"/api/feeds/{created.Id}", update);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteFeed_ShouldReturn404AfterDeletion()
    {
        using var client = await fixture.GetAuthenticatedClient();

        var created = await (await client.PostAsJsonAsync("/api/feeds",
            new CreateFeedCommand() { Title = "Delete me", Content = "To be removed" }))
            .Content.ReadFromJsonAsync<CreateFeedCommandResponse>();
        Assert.NotNull(created);

        var deleteResponse = await client.DeleteAsync(new Uri($"/api/feeds/{created.Id}", UriKind.Relative));
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var secondGet = await client.GetAsync(new Uri($"/api/feeds/{created.Id}", UriKind.Relative));
        Assert.Equal(HttpStatusCode.NotFound, secondGet.StatusCode);
    }
}

