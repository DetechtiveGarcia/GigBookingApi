using GigBookingApi.Application.Dtos;
using GigBookingApi.Application.Interfaces;

namespace GigBookingApi.Infrastructure.Repositories;

public class GigBookingRepository : IGigBookingRepository
{
    public async Task<IEnumerable<GigBookingModel>> GetAllAsync()
    {
        return
        [
            new GigBookingModel(DateTimeOffset.Parse("2024-03-15 18:00+01:00"), DateTimeOffset.Parse("2024-03-15 23:00+01:00"), "Storgatan", "14", "11122", "Stockholm", "Anna Lindgren", "anna@email.com", "0701234567"),
            new GigBookingModel(DateTimeOffset.Parse("2024-04-20 19:00+02:00"), DateTimeOffset.Parse("2024-04-20 23:30+02:00"), "Kungsgatan", "5", "41101", "Göteborg", "Erik Johansson", "erik@email.com", "0709876543"),
            new GigBookingModel(DateTimeOffset.Parse("2024-05-01 20:00+02:00"), DateTimeOffset.Parse("2024-05-01 00:00+02:00"), "Drottninggatan", "22", "21121", "Malmö", "Sara Nilsson", "sara@email.com", "0705551234"),
        ];
    }
}
