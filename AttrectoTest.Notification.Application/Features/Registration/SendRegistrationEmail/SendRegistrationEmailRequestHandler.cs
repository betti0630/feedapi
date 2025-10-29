using AttrectoTest.Notification.Application.Contracts;

using MediatR;


namespace AttrectoTest.Notification.Application.Features.Registration.SendRegistrationEmail;

internal class SendRegistrationEmailRequestHandler(IIamService iamService, IEmailService emailService) : IRequestHandler<SendRegistrationEmailRequest, SendRegistrationEmailResponse>
{
    public async Task<SendRegistrationEmailResponse> Handle(SendRegistrationEmailRequest request, CancellationToken cancellationToken)
    {
        var userData = await iamService.GetUserDataByUserId(request.UserId, cancellationToken);

        if (userData == null)
        {
            return new SendRegistrationEmailResponse
            {
                Success = false,
                Message = $"Not valid userId"
            };
        }

        await emailService.SendEmailAsync(
              to: "beata.dudas@gmail.com",
              subject: "Welcome!",
              body: "Köszönjük, hogy regisztráltál!"
          );

        return new SendRegistrationEmailResponse
        {
            Success = true,
            Message = $"Registration email sent to {userData.UserName}"
        };
    }
}
