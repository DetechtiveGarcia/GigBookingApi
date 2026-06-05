using GigBookingApi.Application.Dtos;

namespace GigBookingApi.Application.Interfaces;
public interface IGigBookingRepository
{
    public Task<IEnumerable<GigBookingDto>> GetAllAsync(CancellationToken ct = default);
    public Task<GigBookingDto> CreateAsync(DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone, CancellationToken ct = default);
    public Task<GigBookingDto> UpdateAsync(string Id, DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone, CancellationToken ct = default);
    public Task<bool> DeleteAsync(string id, CancellationToken ct = default);
}