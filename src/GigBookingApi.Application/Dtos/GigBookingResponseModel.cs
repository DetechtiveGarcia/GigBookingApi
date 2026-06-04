namespace GigBookingApi.Application.Dtos;

public sealed record GigBookingReponseModel(string Id, DateTimeOffset StartDate, DateTimeOffset EndDate, string Street, string StreetNumber, string ZipCode, string City, string ClientName, string ClientEmail, string ClientPhone);