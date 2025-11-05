using System.Text.Json;

namespace FeedApp.BlazorWasm.Services.NotificationBase;

public partial class NotificationsClient : INotificationsClient
{
    public HttpClient HttpClient => _httpClient;

}
