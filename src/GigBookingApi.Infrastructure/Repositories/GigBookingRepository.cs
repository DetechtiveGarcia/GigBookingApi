using GigBookingApi.Application.Dtos;
using GigBookingApi.Application.Exceptions;
using GigBookingApi.Application.Interfaces;
using GigBookingApi.Infrastructure.Entities;

namespace GigBookingApi.Infrastructure.Repositories;

public class GigBookingRepository : IGigBookingRepository
{
    private readonly List<GigBooking> _list = [
            new GigBooking(DateTimeOffset.Parse("2026-07-15 18:00+02:00"), DateTimeOffset.Parse("2026-07-15 23:00+02:00"), "Storgatan", "14", "11122", "Stockholm", "Anna Lindgren", "anna@email.com", "0701234567"),
            new GigBooking(DateTimeOffset.Parse("2026-08-20 19:00+02:00"), DateTimeOffset.Parse("2026-08-20 23:30+02:00"), "Kungsgatan", "5", "41101", "Göteborg", "Erik Johansson", "erik@email.com", "0709876543"),
            new GigBooking(DateTimeOffset.Parse("2026-09-01 20:00+02:00"), DateTimeOffset.Parse("2026-09-01 23:00+02:00"), "Drottninggatan", "22", "21121", "Malmö", "Sara Nilsson", "sara@email.com", "0705551234"),
        ];

    public async Task<GigBookingReponseModel> CreateAsync(DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone)
    {
        var gigBooking = new GigBooking(startDate, endDate, street, streetNumber, zipCode, clientName, clientName, clientEmail, clientPhone);

        //Todo: Contact database
        var responsModel = new GigBookingReponseModel(gigBooking.Id, gigBooking.StartDate, gigBooking.EndDate, gigBooking.Street, gigBooking.StreetNumber, gigBooking.ZipCode, gigBooking.City, gigBooking.ClientName, gigBooking.ClientEmail, gigBooking.ClientPhone);

        return responsModel;
    }


    public async Task<IEnumerable<GigBookingReponseModel>> GetAllAsync()
    {
        var validGigBookingsListResponse = _list.Select(g => new GigBookingReponseModel(g.Id, g.StartDate, g.EndDate, g.Street, g.StreetNumber, g.ZipCode, g.City, g.ClientName, g.ClientEmail, g.ClientPhone));


        return validGigBookingsListResponse;
    }

    public async Task<GigBookingReponseModel> UpdateAsync(string Id, DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone)
    {
        //Todo: await 
        var gigBooking = _list.Find(gb => gb.Id == Id) ?? throw new NotFoundException("No gig found.");
        var validGigBooking = new GigBookingReponseModel(gigBooking.Id, gigBooking.StartDate, gigBooking.EndDate, gigBooking.Street, gigBooking.StreetNumber, gigBooking.ZipCode, gigBooking.City, gigBooking.ClientName, gigBooking.ClientEmail, gigBooking.ClientPhone);


        return validGigBooking;

    }


    public async Task<GigBookingReponseModel> FindByIdAsync(string id)
    {
        //Todo: await 
        var gigBooking = _list.Find(p => p.Id == id) ?? throw new NotFoundException("No gig found.");
        var validGigBooking = new GigBookingReponseModel(gigBooking.Id, gigBooking.StartDate, gigBooking.EndDate, gigBooking.Street, gigBooking.StreetNumber, gigBooking.ZipCode, gigBooking.City, gigBooking.ClientName, gigBooking.ClientEmail, gigBooking.ClientPhone)

        return validGigBooking;
    }
}