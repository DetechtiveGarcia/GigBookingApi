using GigBookingApi.Api.Endpoints;
using GigBookingApi.Api.Middleware;
using GigBookingApi.Api.OpenApi;
using GigBookingApi.Api.Security;
using GigBookingApi.Application.Interfaces;
using GigBookingApi.Application.Services;
using GigBookingApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsConfiguration();
builder.Services.AddOpenApiConfiguration();

builder.Services.AddScoped<IGigBookingRepository, GigBookingRepository>();
builder.Services.AddScoped<IGigBookingService, GigBookingService>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("All");
app.UseHttpsRedirection();

app.MapOpenApiEndpoints();
app.MapGigBookingEndpoints();

app.Run();