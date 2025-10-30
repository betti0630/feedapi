using AttrectoTest.BlazorWeb.Hubs;

using MediatR;

using Microsoft.AspNetCore.SignalR;

namespace AttrectoTest.BlazorWeb.Services.Notification
{
    public class FeedNotificationService:  IFeedNotificationService
    {
        private readonly IHubContext<FeedNotificationHub> _hubContext;

        private static readonly List<Notification> _notifications = new();

        public FeedNotificationService(
            IHubContext<FeedNotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task CreateAsync(Notification notification)
        {
            _notifications.Add(notification);
            await _hubContext.Clients.User(notification.UserId.ToString())
                .SendAsync("ReceiveNotification", notification);
        }

        public Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            var userNotes = _notifications.Where(n => n.UserId == userId);
            return Task.FromResult(userNotes);
        }
    }
}
