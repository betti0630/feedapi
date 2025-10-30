using AttrectoTest.BlazorWeb.Services.Notification;

//using Microsoft.AspNetCore.SignalR.Client;

namespace AttrectoTest.BlazorWeb.Client.Components.NotificationComponent;

public partial class NotificationComponent
{

    //private HubConnection? hubConnection;
    private List<Notification> notifications = new();

  /*  protected override async Task OnInitializedAsync()
    {
        var hubUrl = Navigation.ToAbsoluteUri("/hubs/notifications");

#if WASM
        hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                 ha van auth: pl. JWT token küldése
                 options.AccessTokenProvider = async () => await GetJwtTokenAsync();
            })
            .WithAutomaticReconnect()
            .Build();

        hubConnection.On<Notification>("ReceiveNotification", (notification) =>
        {
            notifications.Add(notification);
            StateHasChanged();
        });

        await hubConnection.StartAsync();
#endif
    }

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
            await hubConnection.DisposeAsync();
    }*/

    private void ToggleNotifications() { }
}

