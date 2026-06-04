namespace GigBookingApi.Infrastructure.Entities;

public sealed class GigBooking(DateTime startDate, DateTime endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone)
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public DateTime StartDate { get; private set; } = startDate;
    public DateTime EndDate { get; private set; } = endDate;
    public string Street { get; private set; } = street;
    public string StreetNumber { get; private set; } = streetNumber;
    public string ZipCode { get; private set; } = zipCode;
    public string City { get; private set; } = city;
    public string ClientName { get; private set; } = clientName;
    public string ClientEmail { get; private set; } = clientEmail;
    public string ClientPhone { get; private set; } = clientPhone;

    public void UpdateGigBooking(DateTime? startDate, DateTime? endDate, string? street, string? streetNumber, string? zipCode, string? city, string? clientName, string? clientEmail, string? clientPhone)
    {
        StartDate = startDate ?? StartDate;
        EndDate = endDate ?? EndDate;
        Street = street ?? Street;
        StreetNumber = streetNumber ?? StreetNumber;
        ZipCode = zipCode ?? ZipCode;
        City = city ?? City;
        ClientName = clientName ?? ClientName;
        ClientEmail = clientName ?? ClientEmail;
        ClientPhone = clientPhone ?? ClientPhone;
    }
}