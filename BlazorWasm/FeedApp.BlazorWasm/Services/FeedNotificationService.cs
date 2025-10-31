using Grpc.Common.Notification;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

using System.Net;

using static Grpc.Common.Notification.NotificationService;

namespace FeedApp.BlazorWasm.Services;

public class FeedNotificationService: IFeedNotificationService
{
    private readonly NotificationServiceClient _client;

    public FeedNotificationService(Uri apiBaseUrl)
    {
        try {
            var httpClienthandler = new HttpClientHandler();
 
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, httpClienthandler);

            var channel = GrpcChannel.ForAddress(apiBaseUrl.ToString(), new GrpcChannelOptions
            {
                HttpHandler = handler,
                HttpVersion = HttpVersion.Version20,
                HttpVersionPolicy = HttpVersionPolicy.RequestVersionExact
            });

            _client = new NotificationServiceClient(channel);

        }
        catch (Exception ex) {
        }
    }

    public async Task SubscribeAsync(string userId, Action<FeedNotificationMessage> onMessage)
    {
        var call = _client.FeedSubscribe(new FeedSubscribeRequest { UserId = userId });

        try
        {
            await foreach (var msg in call.ResponseStream.ReadAllAsync())
            {
                onMessage(msg);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"gRPC stream ended: {ex.Message}");
        }
    }

    public async Task SendAsync(FeedNotificationMessage msg)
    {
        await _client.SendFeedNotificationAsync(msg);
    }
}
