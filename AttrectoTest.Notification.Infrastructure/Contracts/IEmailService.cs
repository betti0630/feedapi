using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttrectoTest.Notification.Infrastructure.Contracts;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}
