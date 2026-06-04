using GigBookingApi.Application.Dtos;

namespace GigBookingApi.Application.Interfaces;
public interface IGigBookingRepository
{
    public IEnumerable<GigBookingModel> GetAllAsync();
}
