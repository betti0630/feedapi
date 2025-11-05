
//using Microsoft.AspNetCore.SignalR.Client;

using Blazored.SessionStorage;

using FeedApp.BlazorWasm.Configuration;
using FeedApp.BlazorWasm.Services;
using FeedApp.BlazorWasm.Services.NotificationBase;

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
    [Inject] INotificationsClient NotificationRestService { get; set; }


    private HubConnection? hubConnection;
    private List<FeedNotificationMessage> notifications = new();
    private List<FeedNotificationDto> messages = new();
    private int _userId;

    protected override async Task OnInitializedAsync()
    {
        var token = await SessionStorage.GetItemAsync<string>("jwt");
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        var identity = user.Identity as ClaimsIdentity;
        var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        _userId = int.Parse(userId);

        var messageList = await NotificationRestService.GetUserNotificationsAsync(_userId);

        messages = messageList.OrderByDescending(m => m.Date).ToList();
        StateHasChanged();

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
            var message = new FeedNotificationDto
            {
                FromUserId = int.Parse(msg.FromUser),
                ToUserId = int.Parse(msg.ToUserId),
                Message = msg.Message,
                Date = DateTime.Now // TODO
            };
            messages.Insert(0, message);
            StateHasChanged();
        });
    }
    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
            await hubConnection.DisposeAsync();
    }

    private bool isOpen;

    private async void ToggleNotifications() { 
        isOpen = !isOpen;    
        if (isOpen)
        {
            if (messages.Any(m => !m.IsRead))
            {
                await NotificationRestService.SetNotificationReadAsync(_userId);
                foreach(var message in messages)
                {
                    message.IsRead = true;
                }
                StateHasChanged();
            }
        }
    }
}
