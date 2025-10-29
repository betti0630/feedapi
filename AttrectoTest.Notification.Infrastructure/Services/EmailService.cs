using AttrectoTest.Notification.Application.Contracts;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using MimeKit;

namespace AttrectoTest.Notification.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly GmailService _gmailService;
    private readonly string _fromEmail;

    public EmailService(string credentialsPath, string fromEmail, string tokenPath = "token.json")
    {
        _fromEmail = fromEmail;

        using var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read);
        var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            new[] { GmailService.Scope.GmailSend },
            "user",
            CancellationToken.None,
            new FileDataStore(tokenPath, true)
        ).Result;


        _gmailService = new GmailService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "NotificationService",
        });
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_fromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var memoryStream = new MemoryStream();
        await message.WriteToAsync(memoryStream);
        var base64UrlEncodedEmail = Convert.ToBase64String(memoryStream.ToArray())
            .Replace('+', '-').Replace('/', '_').Replace("=", "");

        var gmailMessage = new Message { Raw = base64UrlEncodedEmail };
        await _gmailService.Users.Messages.Send(gmailMessage, "me").ExecuteAsync();
    }
}