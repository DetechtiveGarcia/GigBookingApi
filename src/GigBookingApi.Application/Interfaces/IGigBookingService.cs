using GigBookingApi.Application.Dtos;

namespace GigBookingApi.Application.Interfaces;
public interface IGigBookingService
{
    public Task<IEnumerable<GigBookingReponseModel>> GetAllGigBookings();
}
