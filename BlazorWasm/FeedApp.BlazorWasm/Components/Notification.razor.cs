
//using Microsoft.AspNetCore.SignalR.Client;

using Blazored.SessionStorage;

using FeedApp.BlazorWasm.Configuration;
using FeedApp.BlazorWasm.Services;

using Grpc.Common.Notification;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

using System.Security.Claims;

namespace FeedApp.BlazorWasm.Components;

public partial class Notification
{
    [Inject] ISessionStorageService SessionStorage { get; set; }
    [Inject] IOptions<ApiSettings> ApiSettings { get; set; }
    [Inject] IFeedNotificationService NotificationService { get; set;}
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private HubConnection? hubConnection;
    private List<FeedNotificationMessage> notifications = new();
    private List<FeedNotificationMessage> messages = new();

    protected override async Task OnInitializedAsync()
    {
        var token = await SessionStorage.GetItemAsync<string>("jwt");
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        var identity = user.Identity as ClaimsIdentity;
        var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var baseUrl = ApiSettings.Value.FeedApiUrl.TrimEnd('/');
        var hubUrl = $"{baseUrl}/hubs/notifications";
        hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
            })
            .WithAutomaticReconnect()
            .Build();
        hubConnection.On<FeedNotificationMessage>("ReceiveNotification", (notification) =>
        {
            notifications.Add(notification);
            StateHasChanged();
        });
        await hubConnection.StartAsync();

        await NotificationService.SubscribeAsync(userId, msg =>
        {
            messages.Add(msg);
            StateHasChanged();
        });
    }
    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
            await hubConnection.DisposeAsync();
    }

    private void ToggleNotifications() { }
}
