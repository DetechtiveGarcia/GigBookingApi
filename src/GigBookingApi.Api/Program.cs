using GigBookingApi.Api.Endpoints;
using GigBookingApi.Api.OpenApi;
using GigBookingApi.Api.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsConfiguration();
builder.Services.AddOpenApiConfiguration();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("All");
app.UseHttpsRedirection();

app.MapOpenApiEndpoints();
app.MapGigBookingEndpoints();

app.Run();