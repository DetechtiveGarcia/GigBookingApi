using GigBookingApi.Application.Dtos;
using GigBookingApi.Application.Interfaces;

namespace GigBookingApi.Application.Services;

public sealed class GigBookingService(IGigBookingRepository gigBookingRepo) : IGigBookingService
{
    public async Task<GigBookingReponseModel> CreateGigBooking(DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone)
    {
        if (startDate >= endDate)
            throw new ArgumentException("Start date must be before end date");

        if (startDate < DateTimeOffset.Now)
            throw new ArgumentException("Start date cannot be in the past");

        if (string.IsNullOrWhiteSpace(street)) throw new ArgumentException("Street is required");
        if (string.IsNullOrWhiteSpace(streetNumber)) throw new ArgumentException("Street number is required");
        if (string.IsNullOrWhiteSpace(zipCode)) throw new ArgumentException("Zip code is required");
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required");
        if (string.IsNullOrWhiteSpace(clientName)) throw new ArgumentException("Client name is required");
        if (string.IsNullOrWhiteSpace(clientPhone)) throw new ArgumentException("Client phone is required");

        if (string.IsNullOrWhiteSpace(clientEmail) || !clientEmail.Contains('@'))
            throw new ArgumentException("A valid email is required");

        var allBookings = await gigBookingRepo.GetAllAsync();
        if (allBookings.Any(b => startDate < b.EndDate && endDate > b.StartDate))
            throw new InvalidOperationException("The selected time is already booked");

        return await gigBookingRepo.CreateAsync(startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone);
    }

    public async Task<IEnumerable<GigBookingReponseModel>> GetAllGigBookings()
    {
        var allBookings = await gigBookingRepo.GetAllAsync();

        return allBookings;
    }

}