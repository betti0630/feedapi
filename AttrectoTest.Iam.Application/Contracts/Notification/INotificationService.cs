namespace AttrectoTest.Iam.Application.Contracts.Notification;

public interface INotificationService
{
    Task SendRegistrationEmail(int userId);
}
