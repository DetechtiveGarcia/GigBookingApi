using GigBookingApi.Application.Dtos;
using GigBookingApi.Application.Interfaces;
using GigBookingApi.Infrastructure.Entities;

namespace GigBookingApi.Infrastructure.Repositories;

public class GigBookingRepository : IGigBookingRepository
{
    public async Task<GigBookingReponseModel> CreateAsync(DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone)
    {
        var gigBooking = new GigBooking(startDate, endDate, street, streetNumber, zipCode, clientName, clientName, clientEmail, clientPhone);

        //Todo: Contact database
        var responsModel = new GigBookingReponseModel(gigBooking.Id, gigBooking.StartDate, gigBooking.EndDate, gigBooking.Street, gigBooking.StreetNumber, gigBooking.ZipCode, gigBooking.City, gigBooking.ClientName, gigBooking.ClientEmail, gigBooking.ClientPhone);

        return responsModel;
    }

    public async Task<IEnumerable<GigBookingReponseModel>> GetAllAsync()
    {
        return
        [
            new GigBookingReponseModel("1", DateTimeOffset.Parse("2024-03-15 18:00+01:00"), DateTimeOffset.Parse("2024-03-15 23:00+01:00"), "Storgatan", "14", "11122", "Stockholm", "Anna Lindgren", "anna@email.com", "0701234567"),
            new GigBookingReponseModel("2", DateTimeOffset.Parse("2024-04-20 19:00+02:00"), DateTimeOffset.Parse("2024-04-20 23:30+02:00"), "Kungsgatan", "5", "41101", "Göteborg", "Erik Johansson", "erik@email.com", "0709876543"),
            new GigBookingReponseModel("3", DateTimeOffset.Parse("2024-05-01 20:00+02:00"), DateTimeOffset.Parse("2024-05-01 00:00+02:00"), "Drottninggatan", "22", "21121", "Malmö", "Sara Nilsson", "sara@email.com", "0705551234"),
        ];
    }

}