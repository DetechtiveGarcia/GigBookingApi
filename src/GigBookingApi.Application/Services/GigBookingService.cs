using GigBookingApi.Application.Common;
using GigBookingApi.Application.Dtos;
using GigBookingApi.Application.Exceptions;
using GigBookingApi.Application.Interfaces;
using GigBookingApi.Application.Results;

namespace GigBookingApi.Application.Services;

public sealed class GigBookingService(
    IGigBookingRepository gigBookingRepo,
    IEmailService emailService) : IGigBookingService
{
    public async Task<Result<GigBookingDto>> CreateGigBooking(
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string street,
        string streetNumber,
        string zipCode,
        string city,
        string clientName,
        string clientEmail,
        string clientPhone,
        string venue,
        CancellationToken ct = default)
    {
        ValidateBookingRules(startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone, venue);

        var allBookings = await gigBookingRepo.GetAllAsync(ct);
        if (HasBufferConflict(startDate, endDate, allBookings))
            throw new ConflictException("Det måste vara minst 2 timmars paustid mellan spelningar.");

        var created = await gigBookingRepo.CreateAsync(startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone, venue, ct);

        // --- SKICKA MAIL ---
        try
        {
            // 1. Skicka till kunden (Mottagningsbekräftelse)
            var (customerSubject, customerBody) = EmailTemplateHelper.CreateCustomerBookingTemplate(
                created.ClientName,
                created.StartDate,
                created.EndDate,
                created.Venue,
                created.Id);

            await emailService.SendEmailAsync(created.ClientEmail, customerSubject, customerBody, ct);

            // 2. Skicka till Teo / Admin (EmailService har redan koll på AdminEmail från konfigurationen!)
            var fullAddress = $"{created.Street} {created.StreetNumber}, {created.ZipCode} {created.City}";
            var (adminSubject, adminBody) = EmailTemplateHelper.CreateAdminNotificationTemplate(
                created.ClientName,
                created.ClientEmail,
                created.ClientPhone,
                created.StartDate,
                created.EndDate,
                created.Venue,
                fullAddress,
                created.Id);

            await emailService.SendAdminNotificationAsync(adminSubject, adminBody, ct);
        }
        catch (Exception ex)
        {
            // Logga felet om mailet misslyckas så att inte hela bokningsflödet kraschar
            Console.WriteLine($"Kunde inte skicka bekräftelsemail: {ex.Message}");
        }

        return Result<GigBookingDto>.Success(created);
    }

    public async Task<Result<IEnumerable<GigBookingDto>>> GetAllGigBookings(CancellationToken ct)
    {
        var allBookings = await gigBookingRepo.GetAllAsync(ct);

        if (allBookings is null)
        {
            return Result<IEnumerable<GigBookingDto>>.Fail("Error in database");
        }

        return Result<IEnumerable<GigBookingDto>>.Success(allBookings);
    }

    public async Task<Result<GigBookingDto>> UpdateGigBooking(
        string id,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string street,
        string streetNumber,
        string zipCode,
        string city,
        string clientName,
        string clientEmail,
        string clientPhone,
        string venue,
        CancellationToken ct)
    {
        ValidateBookingRules(startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone, venue);

        var allBookings = await gigBookingRepo.GetAllAsync(ct);
        if (HasBufferConflict(startDate, endDate, allBookings, currentBookingId: id))
            throw new ConflictException("Det måste vara minst 2 timmars paustid mellan spelningar.");

        var updated = await gigBookingRepo.UpdateAsync(id, startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone, venue, ct);

        if (updated is null)
            return Result<GigBookingDto>.Fail("No gig found");

        // --- SKICKA UPPDATERINGSE-POST ---
        try
        {
            // 1. Mail till Kunden
            var subject = "Din bokning har uppdaterats";
            var body = $@"
        <h2>Uppdaterad Bokning</h2>
        <p>Hej {updated.ClientName},</p>
        <p>Din spelning har blivit uppdaterad med följande tider:</p>
        <p><strong>Datum & Tid:</strong> {updated.StartDate:yyyy-MM-dd HH:mm} - {updated.EndDate:HH:mm}</p>
        <p><strong>Spelplats:</strong> {updated.Venue}</p>";

            await emailService.SendEmailAsync(updated.ClientEmail, subject, body, ct);

            // 2. Mail till Admin/Teo
            var (adminSubject, adminBody) = EmailTemplateHelper.UpdateAdminNotificationTemplate(
                updated.ClientName,
                updated.ClientEmail,
                updated.StartDate,
                updated.EndDate,
                updated.Venue,
                updated.Id
            );
            await emailService.SendAdminNotificationAsync(adminSubject, adminBody, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Kunde inte skicka uppdateringsmail: {ex.Message}");
        }

        return Result<GigBookingDto>.Success(updated);
    }

    public async Task<Result> DeleteGigBooking(string id, CancellationToken ct)
    {
        // Hämta bokningen innan radering för att veta vem som ska få avbokningsmailet
        var allBookings = await gigBookingRepo.GetAllAsync(ct);
        var bookingToDelete = allBookings.FirstOrDefault(b => b.Id == id);

        var isDelete = await gigBookingRepo.DeleteAsync(id, ct);

        if (!isDelete)
            return Result.Fail("Can't delete gig booking.");

        // --- SKICKA AVBOKNINGSE-POST ---
        if (bookingToDelete is not null)
        {
            try
            {
                // 1. Mail till Kunden
                var subject = "Din bokning har avbokats";
                var body = $@"
            <h2>Avbokningsbekräftelse</h2>
            <p>Hej {bookingToDelete.ClientName},</p>
            <p>Din bokning (ID: {bookingToDelete.Id}) för spelningen på {bookingToDelete.Venue} har avbokats.</p>";

                await emailService.SendEmailAsync(bookingToDelete.ClientEmail, subject, body, ct);

                // 2. Mail till Admin/Teo
                var (adminSubject, adminBody) = EmailTemplateHelper.DeleteAdminNotificationTemplate(
                    bookingToDelete.ClientName,
                    bookingToDelete.Venue,
                    bookingToDelete.Id
                );
                await emailService.SendAdminNotificationAsync(adminSubject, adminBody, ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kunde inte skicka avbokningsmail: {ex.Message}");
            }
        }

        return Result.Success();
    }

    #region Private Helper Methods

    private static void ValidateBookingRules(
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string street,
        string streetNumber,
        string zipCode,
        string city,
        string clientName,
        string clientEmail,
        string clientPhone,
        string venue)
    {
        if (startDate >= endDate)
            throw new ValidationException("Start date must be before end date");

        if (startDate < DateTimeOffset.Now)
            throw new ValidationException("Start date cannot be in the past");

        if (string.IsNullOrWhiteSpace(street)) throw new ValidationException("Street is required");
        if (string.IsNullOrWhiteSpace(streetNumber)) throw new ValidationException("Street number is required");
        if (string.IsNullOrWhiteSpace(zipCode)) throw new ValidationException("Zip code is required");
        if (string.IsNullOrWhiteSpace(city)) throw new ValidationException("City is required");
        if (string.IsNullOrWhiteSpace(clientName)) throw new ValidationException("Client name is required");
        if (string.IsNullOrWhiteSpace(clientPhone)) throw new ValidationException("Client phone is required");
        if (string.IsNullOrWhiteSpace(venue)) throw new ValidationException("Venue is required");

        if (string.IsNullOrWhiteSpace(clientEmail) || !clientEmail.Contains('@'))
            throw new ValidationException("A valid email is required");

        var dayOfWeek = startDate.DayOfWeek;
        var startTime = startDate.TimeOfDay;
        var endTime = endDate.TimeOfDay;

        if (endDate.Date > startDate.Date && endTime == TimeSpan.Zero)
        {
            endTime = TimeSpan.FromHours(24);
        }

        switch (dayOfWeek)
        {
            case DayOfWeek.Friday:
                if (startTime < TimeSpan.FromHours(20) || endTime > TimeSpan.FromHours(23))
                    throw new ValidationException("Fredagsspelningar kan endast bokas mellan 20:00 och 23:00.");
                break;

            case DayOfWeek.Saturday:
                if (startTime < TimeSpan.FromHours(16) || endTime > TimeSpan.FromHours(24))
                    throw new ValidationException("Lördagsspelningar kan endast bokas mellan 16:00 och 00:00.");
                break;

            case DayOfWeek.Sunday:
                if (startTime < TimeSpan.FromHours(12) || endTime > TimeSpan.FromHours(20))
                    throw new ValidationException("Söndagsspelningar kan endast bokas mellan 12:00 och 20:00.");
                break;

            default:
                throw new ValidationException("Bokningar kan endast göras på fredagar, lördagar och söndagar.");
        }
    }

    private static bool HasBufferConflict(
        DateTimeOffset newStart,
        DateTimeOffset newEnd,
        IEnumerable<GigBookingDto> existingBookings,
        string? currentBookingId = null)
    {
        var buffer = TimeSpan.FromHours(2);

        foreach (var b in existingBookings)
        {
            if (b.Id == currentBookingId)
                continue;

            bool overlapsWithBuffer = newStart < b.EndDate.Add(buffer) &&
                                      newEnd.Add(buffer) > b.StartDate;

            if (overlapsWithBuffer)
                return true;
        }

        return false;
    }

    #endregion
}