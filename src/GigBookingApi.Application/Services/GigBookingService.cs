using GigBookingApi.Application.Dtos;
using GigBookingApi.Application.Exceptions;
using GigBookingApi.Application.Interfaces;
using GigBookingApi.Application.Results;

namespace GigBookingApi.Application.Services;

public sealed class GigBookingService(IGigBookingRepository gigBookingRepo) : IGigBookingService
{
    public async Task<Result<GigBookingDto>> CreateGigBooking(DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone, string venue, CancellationToken ct = default)
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

        var allBookings = await gigBookingRepo.GetAllAsync(ct);
        if (allBookings.Any(b => startDate < b.EndDate && endDate > b.StartDate))
            throw new ConflictException("The selected time is already booked");

        var created = await gigBookingRepo.CreateAsync(startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone, venue, ct);


        return Result<GigBookingDto>.Success(created);
    }

    public async Task<Result<IEnumerable<GigBookingDto>>> GetAllGigBookings(CancellationToken ct)
    {
        var allBookings = await gigBookingRepo.GetAllAsync(ct);

        if(allBookings is null)
        {
            return Result<IEnumerable<GigBookingDto>>.Fail("Error in database");
        }

        return Result<IEnumerable<GigBookingDto>>.Success(allBookings);
    }

    public async Task<Result> UpdateGigBooking(string id, DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone, string venue, CancellationToken ct)
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

        var allBookings = await gigBookingRepo.GetAllAsync(ct);
        if (allBookings.Any(b => b.Id != id && startDate < b.EndDate && endDate > b.StartDate))
            throw new ConflictException("The selected time is already booked");

        var created = await gigBookingRepo.UpdateAsync(id, startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone, venue, ct);

        if (created is null)
            return Result.Fail("No gig found");

        return Result.Success();
    }

    public async Task<Result> DeleteGigBooking(string id, CancellationToken ct)
    {
        var isDelete = await gigBookingRepo.DeleteAsync(id, ct);

        if (!isDelete)
            return Result.Fail("Can't delete gig booking.");

        return Result.Success();
    }

}