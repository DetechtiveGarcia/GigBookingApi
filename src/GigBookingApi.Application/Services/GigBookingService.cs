using GigBookingApi.Application.Dtos;
using GigBookingApi.Application.Exceptions;
using GigBookingApi.Application.Interfaces;
using GigBookingApi.Application.Results;

namespace GigBookingApi.Application.Services;

public sealed class GigBookingService(IGigBookingRepository gigBookingRepo) : IGigBookingService
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
        // 1. Kör alla bas- och tidsvalideringar
        ValidateBookingRules(startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone, venue);

        // 2. Kontrollera överlapp med 2 timmars buffert (exkludera nuvarande bokning)
        var allBookings = await gigBookingRepo.GetAllAsync(ct);
        if (HasBufferConflict(startDate, endDate, allBookings, currentBookingId: id))
            throw new ConflictException("Det måste vara minst 2 timmars paustid mellan spelningar.");

        var updated = await gigBookingRepo.UpdateAsync(id, startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone, venue, ct);

        if (updated is null)
            return Result<GigBookingDto>.Fail("No gig found");

        return Result<GigBookingDto>.Success(updated);
    }

    public async Task<Result> DeleteGigBooking(string id, CancellationToken ct)
    {
        var isDelete = await gigBookingRepo.DeleteAsync(id, ct);

        if (!isDelete)
            return Result.Fail("Can't delete gig booking.");

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

        // --- TIDS- OCH DAGSVALIDERINGSREGLER ---
        var dayOfWeek = startDate.DayOfWeek;
        var startTime = startDate.TimeOfDay;
        var endTime = endDate.TimeOfDay;

        // Om bokningen slutar vid midnatt (00:00 nästa dygns start)
        if (endDate.Date > startDate.Date && endTime == TimeSpan.Zero)
        {
            endTime = TimeSpan.FromHours(24);
        }

        switch (dayOfWeek)
        {
            case DayOfWeek.Friday:
                // Fredag: 20:00 - 23:00
                if (startTime < TimeSpan.FromHours(20) || endTime > TimeSpan.FromHours(23))
                    throw new ValidationException("Fredagsspelningar kan endast bokas mellan 20:00 och 23:00.");
                break;

            case DayOfWeek.Saturday:
                // Lördag: 16:00 - 00:00 (24:00)
                if (startTime < TimeSpan.FromHours(16) || endTime > TimeSpan.FromHours(24))
                    throw new ValidationException("Lördagsspelningar kan endast bokas mellan 16:00 och 00:00.");
                break;

            case DayOfWeek.Sunday:
                // Söndag: 12:00 - 20:00
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

            // Krockar ny bokning med en existerande bokning + 2h paustid åt båda hållen?
            bool overlapsWithBuffer = newStart < b.EndDate.Add(buffer) &&
                                      newEnd.Add(buffer) > b.StartDate;

            if (overlapsWithBuffer)
                return true;
        }

        return false;
    }

    #endregion
}