using GigBookingApi.Application.Dtos;
using GigBookingApi.Application.Interfaces;

namespace GigBookingApi.Application.Services;

public sealed class GigBookingService(IGigBookingRepository gigBookingRepo) : IGigBookingService
{
    public async Task<IEnumerable<GigBookingReponseModel>> GetAllGigBookings()
    {
        var allBookings = await gigBookingRepo.GetAllAsync();

        return allBookings;
    }

}