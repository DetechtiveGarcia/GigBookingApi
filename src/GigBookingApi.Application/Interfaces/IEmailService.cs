namespace GigBookingApi.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
    Task SendAdminNotificationAsync(string subject, string htmlBody, CancellationToken ct = default);
}