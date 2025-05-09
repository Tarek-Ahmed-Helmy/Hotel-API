namespace Utilities;

public interface IEmailService
{
    Task SendAsync(EmailDto mail);
}

