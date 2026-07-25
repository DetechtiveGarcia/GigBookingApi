using Azure;
using Azure.Communication.Email;
using GigBookingApi.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace GigBookingApi.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly EmailClient _emailClient;
    private readonly EmailSettings _settings;

    public EmailService(
        IOptions<EmailSettings> emailSettings,
        EmailClient emailClient)
    {
        _settings = emailSettings.Value;
        _emailClient = emailClient;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
    {
        var emailContent = new EmailContent(subject) { Html = htmlBody };
        var message = new EmailMessage(_settings.From, to, emailContent);

        try
        {
            await _emailClient.SendAsync(WaitUntil.Started, message, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Kunde inte skicka mail till {to}: {ex.Message}");
        }
    }

    public async Task SendAdminNotificationAsync(string subject, string htmlBody, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_settings.AdminEmail))
        {
            Console.WriteLine("AdminEmail saknas i konfigurationen.");
            return;
        }

        await SendEmailAsync(_settings.AdminEmail, subject, htmlBody, ct);
    }
}