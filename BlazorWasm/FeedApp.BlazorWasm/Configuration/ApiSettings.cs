namespace FeedApp.BlazorWasm.Configuration;

internal class ApiSettings
{
    public string IamApiUrl { get; set; } = string.Empty;
    public string FeedApiUrl { get; set; } = string.Empty;
    public Uri? NotificationUrl { get; set; }
}
