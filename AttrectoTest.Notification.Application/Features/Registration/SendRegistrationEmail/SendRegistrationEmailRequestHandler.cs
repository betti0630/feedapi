using AttrectoTest.Notification.Application.Contracts;

using MediatR;


namespace AttrectoTest.Notification.Application.Features.Registration.SendRegistrationEmail;

internal class SendRegistrationEmailRequestHandler(IEmailService emailService) : IRequestHandler<SendRegistrationEmailRequest, SendRegistrationEmailResponse>
{
    public async Task<SendRegistrationEmailResponse> Handle(SendRegistrationEmailRequest request, CancellationToken cancellationToken)
    {
        await emailService.SendEmailAsync(
              to: "beata.dudas@gmail.com",
              subject: "Welcome!",
              body: "Köszönjük, hogy regisztráltál!"
          );

        return new SendRegistrationEmailResponse
        {
            Success = true,
            Message = $"Registration email sent to {request.UserId}"
        };
    }
}
