using GigBookingApi.Application.Dtos;
using GigBookingApi.Application.Results;

namespace GigBookingApi.Application.Interfaces;
public interface IGigBookingService
{
    public Task<Result<IEnumerable<GigBookingReponseModel>>> GetAllGigBookings();
    public Task<Result<GigBookingReponseModel>> CreateGigBooking(DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone);
    public Task<Result<GigBookingReponseModel>> UpdateGigBooking(string id, DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone);
    public Task<Result<GigBookingReponseModel>> FindGigBookingById(string id);
}