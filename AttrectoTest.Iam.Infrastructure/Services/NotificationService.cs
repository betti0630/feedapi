using AttrectoTest.Common.Grpc.Notification;
using AttrectoTest.Iam.Application.Contracts.Notification;

using Grpc.Net.Client;

using System.Net;

using static AttrectoTest.Common.Grpc.Notification.NotificationService;

namespace AttrectoTest.Iam.Infrastructure.Services;

internal class NotificationService : INotificationService
{
    private readonly NotificationServiceClient _client;

    public NotificationService(Uri notificationApiUrl)
    {
        var handler = new HttpClientHandler
        {
            // Ha self-signed cert van (pl. dev Docker környezet)
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var channel = GrpcChannel.ForAddress(notificationApiUrl, new GrpcChannelOptions
        {
            HttpHandler = handler,
            HttpVersion = HttpVersion.Version20,
            HttpVersionPolicy = HttpVersionPolicy.RequestVersionExact
        });

        _client = new NotificationServiceClient(channel);
    }


    public async Task SendRegistrationEmail(int userId)
    {
        var request = new RegistrationEmailRequest { UserId = userId };
        await _client.SendRegistrationEmailAsync(request);
    }
}
