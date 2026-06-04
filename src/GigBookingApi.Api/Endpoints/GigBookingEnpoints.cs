namespace GigBookingApi.Api.Endpoints;

public static class GigBookingEnpoints
{
    public static void MapGigBookingEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/gigbooking")
            .WithTags("Git Booking Enpoints")
            .WithDescription("Demo on how to use Gig Booking API");
    }
}