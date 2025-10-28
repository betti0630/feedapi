using AttrectoTest.Common.Grpc.Notification;
using AttrectoTest.Notification.Infrastructure.Services;


using Grpc.Core;

namespace AttrectoTest.NotificationService.Services;

public class NotificationServiceImpl : AttrectoTest.Common.Grpc.Notification.NotificationService.NotificationServiceBase
{
    private readonly ILogger<NotificationServiceImpl> _logger;

    public NotificationServiceImpl(ILogger<NotificationServiceImpl> logger)
    {
        _logger = logger;
       
    }

    public override async Task<NotificationResponse> SendRegistrationEmail(RegistrationEmailRequest request, ServerCallContext context)
    {
        //_logger.LogInformation($"[Email] Küldés: {request.Email} - Üdvözlünk, {request.UserName}!");

        var emailService = new EmailService(
            credentialsPath: "credentials_desktop.json",
            fromEmail: "beata.dudas@attrecto.com"
        );

        await emailService.SendEmailAsync(
            to: "beata.dudas@gmail.com",
            subject: "Welcome!",
            body: "Köszönjük, hogy regisztráltál!"
        );

        return new NotificationResponse
        {
            Success = true,
            Message = $"Registration email sent to {request.UserId}"
        };
    }

    public override Task<NotificationResponse> SendFeedNotification(FeedNotificationRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"[Notification] {request.Type} feedId={request.FeedId} from={request.FromUserId} to={request.ToUserId}");

        return Task.FromResult(new NotificationResponse
        {
            Success = true,
            Message = $"Feed notification ({request.Type}) sent."
        });
    }
}
