using GigBookingApi.Application.Dtos;

namespace GigBookingApi.Application.Interfaces;
public interface IGigBookingRepository
{
    public Task<IEnumerable<GigBookingReponseModel>> GetAllAsync();
    public Task<GigBookingReponseModel> CreateAsync(DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone);
    public Task<GigBookingReponseModel> UpdateAsync(string Id, DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone)
    public Task<bool> DeleteAsync(string id);
}