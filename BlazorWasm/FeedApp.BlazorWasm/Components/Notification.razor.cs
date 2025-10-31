
//using Microsoft.AspNetCore.SignalR.Client;

using Blazored.SessionStorage;

using FeedApp.BlazorWasm.Configuration;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace FeedApp.BlazorWasm.Components;

public partial class Notification
{
    [Inject] ISessionStorageService SessionStorage { get; set; }
    [Inject] IOptions<ApiSettings> ApiSettings { get; set; }

    private HubConnection? hubConnection;
    private List<Notification> notifications = new();

    protected override async Task OnInitializedAsync()
    {
        var token = await SessionStorage.GetItemAsync<string>("jwt");

        var baseUrl = ApiSettings.Value.FeedApiUrl.TrimEnd('/');
        var hubUrl = $"{baseUrl}/hubs/notifications";
        hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
            })
            .WithAutomaticReconnect()
            .Build();
        hubConnection.On<Notification>("ReceiveNotification", (notification) =>
        {
            notifications.Add(notification);
            StateHasChanged();
        });
        await hubConnection.StartAsync();
    }
    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
            await hubConnection.DisposeAsync();
    }

    private void ToggleNotifications() { }
}
