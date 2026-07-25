using Azure.Communication.Email;
using GigBookingApi.Api.Endpoints;
using GigBookingApi.Api.Middleware;
using GigBookingApi.Api.OpenApi;
using GigBookingApi.Api.Security;
using GigBookingApi.Application.Interfaces;
using GigBookingApi.Application.Services;
using GigBookingApi.Infrastructure;
using GigBookingApi.Infrastructure.Contexts;
using GigBookingApi.Infrastructure.Email;
using GigBookingApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsConfiguration();
builder.Services.AddOpenApiConfiguration();
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton(x =>
    new EmailClient(builder.Configuration["CommunicationServices:ConnectionString"]));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IGigBookingRepository, GigBookingRepository>();
builder.Services.AddScoped<IGigBookingService, GigBookingService>();
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();


builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("All");
app.UseHttpsRedirection();

app.MapOpenApiEndpoints();
app.MapGigBookingEndpoints();

app.Run();