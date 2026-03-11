using System.Net;
using System.Net.Mail;
using auradocs_api.Data;
using Microsoft.Extensions.Options;

namespace auradocs_api.Services;

public interface IEmailService
{
    public Task<bool> SendEmailAsync(string toEmail,
        string subject,
        string htmlBody);
}
public class EmailService: IEmailService
{
    private readonly EmailSettings _settings;
    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task<bool> SendEmailAsync(
        string toEmail,
        string subject,
        string htmlBody
    )
    {
        MailMessage message = new MailMessage
        {
            From = new MailAddress(
                _settings.SenderEmail,
                _settings.SenderName
            ),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        message.To.Add(toEmail);
        using SmtpClient smtp = new SmtpClient(_settings.SmtpServer,_settings.Port)
        {
            Credentials = new NetworkCredential(
                _settings.Username,
                _settings.Password
            ),
            EnableSsl = true
        };

        await smtp.SendMailAsync(message);
        return true;
    }
}