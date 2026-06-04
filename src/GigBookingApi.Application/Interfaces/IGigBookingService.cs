using GigBookingApi.Application.Dtos;

namespace GigBookingApi.Application.Interfaces;
public interface IGigBookingService
{
    public Task<IEnumerable<GigBookingReponseModel>> GetAllGigBookings();
    public Task<GigBookingReponseModel> CreateGigBooking(DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone);
}