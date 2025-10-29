using MediatR;

namespace AttrectoTest.Notification.Application.Features.Registration.SendRegistrationEmail;

public class SendRegistrationEmailRequest : IRequest<SendRegistrationEmailResponse>
{
    public int UserId { get; set;}
}

public class SendRegistrationEmailResponse
{
    public bool Success { get; set;}
    public string? Message { get; set;}
}
