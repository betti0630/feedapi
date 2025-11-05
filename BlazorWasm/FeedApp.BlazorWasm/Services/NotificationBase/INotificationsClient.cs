namespace FeedApp.BlazorWasm.Services.NotificationBase;

public partial interface INotificationsClient
{
    public HttpClient HttpClient { get; }
}
