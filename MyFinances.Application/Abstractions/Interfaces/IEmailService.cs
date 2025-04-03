namespace MyFinances.Application.Abstractions.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}