using System.Net;
using System.Net.Mail;
using MyFinances.Application.Abstractions.Services;

namespace MyFinances.Infrastructure.Notification;

public class EmailService : IEmailService
{
    private readonly EmailServiceOptions _emailServiceOptions;

    public EmailService(EmailServiceOptions emailServiceOptions)
    {
        _emailServiceOptions = emailServiceOptions;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpClient = new SmtpClient(_emailServiceOptions.Host)
        {
            Port = _emailServiceOptions.SmtpPort,
            Credentials = new NetworkCredential(_emailServiceOptions.Sender, _emailServiceOptions.Password),
            EnableSsl = true
        };

        var mailMessage = new MailMessage(
            _emailServiceOptions.Sender,
            to,
            subject,
            body);

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
        }
    }
}

public class EmailServiceOptions
{
    public string Sender { get; set; }
    public string Password { get; set; }
    public int SmtpPort { get; set; }
    public string Host { get; set; }
}