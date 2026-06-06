using GigBookingApi.Application.Dtos;
using GigBookingApi.Application.Results;

namespace GigBookingApi.Application.Interfaces;
public interface IGigBookingService
{
    public Task<Result<IEnumerable<GigBookingDto>>> GetAllGigBookings(CancellationToken ct = default);
    public Task<Result<GigBookingDto>> CreateGigBooking(DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone, string venue, CancellationToken ct = default);
    public Task<Result> UpdateGigBooking(string id, DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone, string venue, CancellationToken ct = default);
    public Task<Result> DeleteGigBooking(string id, CancellationToken ct = default);
}