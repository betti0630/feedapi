using Grpc.Common.Notification;

namespace FeedApp.BlazorWasm.Services;

public interface IFeedNotificationService
{
    Task SubscribeAsync(string userId, Action<FeedNotificationMessage> onMessage);
}
