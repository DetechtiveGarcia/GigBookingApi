using GigBookingApi.Application.Dtos;
using GigBookingApi.Application.Exceptions;
using GigBookingApi.Application.Interfaces;
using GigBookingApi.Application.Results;

namespace GigBookingApi.Application.Services;

public sealed class GigBookingService(IGigBookingRepository gigBookingRepo) : IGigBookingService
{
    public async Task<Result<GigBookingReponseModel>> CreateGigBooking(DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone)
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

        if (string.IsNullOrWhiteSpace(clientEmail) || !clientEmail.Contains('@'))
            throw new ValidationException("A valid email is required");

        var allBookings = await gigBookingRepo.GetAllAsync();
        if (allBookings.Any(b => startDate < b.EndDate && endDate > b.StartDate))
            throw new ConflictException("The selected time is already booked");

        var created = await gigBookingRepo.CreateAsync(startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone);


        return Result<GigBookingReponseModel>.Success(created);
    }

    public async Task<Result<IEnumerable<GigBookingReponseModel>>> GetAllGigBookings()
    {
        var allBookings = await gigBookingRepo.GetAllAsync();

        if(allBookings is null)
        {
            return Result<IEnumerable<GigBookingReponseModel>>.Fail("Error in database");
        }

        return Result<IEnumerable<GigBookingReponseModel>>.Success(allBookings);
    }

    public async Task<Result<GigBookingReponseModel>> UpdateGigBooking(string id, DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone)
    {
 
        var gigBooking = await gigBookingRepo.UpdateAsync(id, startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone);

        if (gigBooking is null)
            return Result<GigBookingReponseModel>.Fail("No gig found");





        return Result<GigBookingReponseModel>.Success(gigBooking);
    }

    public Task<Result<GigBookingReponseModel>> FindGigBookingById(string id)
    {
        //Todo: await

        throw new NotImplementedException();
    }
}