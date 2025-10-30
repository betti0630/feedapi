namespace AttrectoTest.BlazorWeb.Services.Notification;

public interface IFeedNotificationService
{
    Task CreateAsync(Notification notification);
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
}
