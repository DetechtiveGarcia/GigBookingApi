namespace GigBookingApi.Application.Interfaces;

public interface IEmailService
{
    Task SendBookingConfirmationAsync(string to, string subject, string htmlBody, CancellationToken ct);
}
