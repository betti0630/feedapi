using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Common.Grpc.Iam;

using Grpc.Net.Client;

using System.Net;

using static AttrectoTest.Common.Grpc.Iam.IamService;

namespace AttrectoTest.BlazorWeb.Services;

internal class IamGrpcService : IIamService
{
    private readonly IamServiceClient _client;

    public IamGrpcService(string iamApiUrl)
    {
        var handler = new HttpClientHandler
        {
            // Ha self-signed cert van (pl. dev Docker környezet)
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var channel = GrpcChannel.ForAddress(iamApiUrl, new GrpcChannelOptions
        {
            HttpHandler = handler,
            HttpVersion = HttpVersion.Version20,
            HttpVersionPolicy = HttpVersionPolicy.RequestVersionExact
        });

        _client = new IamServiceClient(channel);
    }


    public async Task<int> GetUserIdByUserName(string userName, CancellationToken ct = default)
    {
        var result = await _client.GetUserIdByUserNameAsync(new GetUserIdByUserNameRequest { UserName = userName }, cancellationToken: ct);
        return result.UserId;
    }

    public async Task<string> GetUserNameByUserId(int userId, CancellationToken ct = default)
    {
        var result = await _client.GetUserDataAsync(new GetUserDataRequest { UserId = userId }, cancellationToken: ct);
        return result.UserName;
    }
}
