using GigBookingApi.Api.Dtos;
using GigBookingApi.Api.Dtos.Requests;
using GigBookingApi.Application.Interfaces;

namespace GigBookingApi.Api.Endpoints;

public static class GigBookingEnpoints
{
    public static void MapGigBookingEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/gigbooking")
            .WithTags("Git Booking Enpoints")
            .WithDescription("Demo on how to use Gig Booking API");

        group.MapGet("/all-bookings", GetAllBookings);
        group.MapPost("/create", CreateGigBooking);
        group.MapPut("/update/{id}", UpdateGigBooking);
        group.MapDelete("/delete/{id}", DeleteGigBooking);

    }

    private static async Task<IResult> GetAllBookings(IGigBookingService gigBookingService)
    {
        var result = await gigBookingService.GetAllGigBookings();

        if (!result.Succeeded)
            return Results.BadRequest(result.ErrorMessage);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> CreateGigBooking(CreateGigBookingRequest request, IGigBookingService gigBookingService)
    {
        var result = await gigBookingService.CreateGigBooking(request.StartDate, request.EndDate, request.Street, request.StreetNumber, request.ZipCode, request.City, request.ClientName, request.ClientEmail, request.ClientPhone);

        if (!result.Succeeded)
            return Results.BadRequest(result.ErrorMessage);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> UpdateGigBooking(string id, UpdateGigBookingRequest request, IGigBookingService gigBookingService)
    {
        var result = await gigBookingService.UpdateGigBooking(id, request.StartDate, request.EndDate, request.Street, request.StreetNumber, request.ZipCode, request.City, request.ClientName, request.ClientEmail, request.ClientPhone);

        if (!result.Succeeded)
            return Results.BadRequest(result.ErrorMessage);

        return Results.Ok();
    }

    private static async Task<IResult> DeleteGigBooking(string id, IGigBookingService gigBookingService)
    {
        var result = await gigBookingService.DeleteGigBooking(id);

        if (!result.Succeeded)
            return Results.Conflict();

        return Results.Ok();
    }
}