using AttrectoTest.Common.Grpc.Notification;
using AttrectoTest.Notification.Infrastructure.Contracts;

using Grpc.Core;

using Microsoft.Extensions.Logging;

namespace AttrectoTest.NotificationService.Services;

public class NotificationServiceImpl : AttrectoTest.Common.Grpc.Notification.NotificationService.NotificationServiceBase
{
    private readonly ILogger<NotificationServiceImpl> _logger;
    private readonly IEmailService _emailService;

    public NotificationServiceImpl(ILogger<NotificationServiceImpl> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public override async Task<NotificationResponse> SendRegistrationEmail(RegistrationEmailRequest request, ServerCallContext context)
    {

        await _emailService.SendEmailAsync(
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
