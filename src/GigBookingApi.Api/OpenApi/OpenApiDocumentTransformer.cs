using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace GigBookingApi.Api.OpenApi;

public sealed class OpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();

        document.Info.Description = """
            ## Introduction

            he Gig Booking API provides endpoints for managing musician bookings. It allows clients to view available dates, create new bookings, and manage existing reservations.

            """;

        return Task.CompletedTask;
    }
}
