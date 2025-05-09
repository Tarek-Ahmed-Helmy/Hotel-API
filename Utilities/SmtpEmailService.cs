using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Utilities;

public class SmtpEmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public SmtpEmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendAsync(EmailDto mail)
    {
        using var message = new MailMessage();
        message.To.Add(new MailAddress(mail.To));
        message.From = new MailAddress(_settings.SenderEmail, _settings.SenderName);
        message.Subject = mail.Subject;
        message.Body = mail.Body;
        message.IsBodyHtml = mail.IsHtml;

        using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.Username, _settings.Password),
            EnableSsl = true
        };

        await client.SendMailAsync(message);
    }
}

