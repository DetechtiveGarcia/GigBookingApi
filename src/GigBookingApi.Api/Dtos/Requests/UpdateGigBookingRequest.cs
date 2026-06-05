namespace GigBookingApi.Api.Dtos.Requests;

public sealed record UpdateGigBookingRequest(string Id, DateTimeOffset StartDate, DateTimeOffset EndDate, string Street, string StreetNumber, string ZipCode, string City, string ClientName, string ClientEmail, string ClientPhone);