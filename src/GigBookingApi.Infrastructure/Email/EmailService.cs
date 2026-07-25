using Azure.Communication.Email;
using GigBookingApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace GigBookingApi.Infrastructure.Email;
public class EmailService : IEmailService
{
    private readonly EmailClient _emailClient;
    private readonly string _from;

    public EmailService(
        IOptions<EmailSettings> emailSettings,
        IConfiguration configuration)
    {
        _from = emailSettings.Value.From;

        var connectionString = configuration["CommunicationServices:ConnectionString"];

        _emailClient = new EmailClient(connectionString);
    }

    public async Task SendBookingConfirmationAsync(string to, string subject, string htmlBody, CancellationToken ct)
    {
        var emailContent = new EmailContent(subject)
        {
            Html = htmlBody
        };

        var recipients = new EmailRecipients(new[]
        {
            new EmailAddress(to)
        });

        var message = new EmailMessage(_from, recipients, emailContent);

        await _emailClient.SendAsync(Azure.WaitUntil.Completed, message, ct);
    }
}
