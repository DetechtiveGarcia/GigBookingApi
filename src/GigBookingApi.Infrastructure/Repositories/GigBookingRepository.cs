using GigBookingApi.Application.Dtos;
using GigBookingApi.Application.Exceptions;
using GigBookingApi.Application.Interfaces;
using GigBookingApi.Infrastructure.Contexts;
using GigBookingApi.Infrastructure.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GigBookingApi.Infrastructure.Repositories;

public class GigBookingRepository(IMongoDbContext context, IOptions<MongoDbSettings> settings) : IGigBookingRepository
{

    private readonly IMongoCollection<GigBooking> _collection = context.GetCollection<GigBooking>(settings.Value.CollectionName);

    public async Task<GigBookingDto> CreateAsync(DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone, string venue, CancellationToken ct)
    {
        var gigBooking = new GigBooking(startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone, venue);

        await _collection.InsertOneAsync(gigBooking, options: null, ct);
        var gigBookingDto = new GigBookingDto(gigBooking.Id, gigBooking.StartDate, gigBooking.EndDate, gigBooking.Street, gigBooking.StreetNumber, gigBooking.ZipCode, gigBooking.City, gigBooking.ClientName, gigBooking.ClientEmail, gigBooking.ClientPhone, gigBooking.Venue);

        return gigBookingDto;
    }


    public async Task<IEnumerable<GigBookingDto>> GetAllAsync(CancellationToken ct)
    {
        var allBookings = await _collection.Find(new BsonDocument()).ToListAsync(ct);
        return allBookings.Select(g => new GigBookingDto(g.Id, g.StartDate, g.EndDate, g.Street, g.StreetNumber, g.ZipCode, g.City, g.ClientName, g.ClientEmail, g.ClientPhone, g.Venue));
    }

    public async Task<GigBookingDto> UpdateAsync(string id, DateTimeOffset startDate, DateTimeOffset endDate, string street, string streetNumber, string zipCode, string city, string clientName, string clientEmail, string clientPhone, string venue, CancellationToken ct)
    {

        var gigBooking = await FindByIdAsync(id, ct);



        gigBooking.UpdateGigBooking(startDate, endDate, street, streetNumber, zipCode, city, clientName, clientEmail, clientPhone, venue);

        FilterDefinition<GigBooking> filter = Builders<GigBooking>.Filter.Eq("Id", gigBooking.Id);



        await _collection.ReplaceOneAsync(filter, gigBooking, cancellationToken: ct);

        var gigBookingDto = new GigBookingDto(gigBooking.Id, gigBooking.StartDate, gigBooking.EndDate, gigBooking.Street, gigBooking.StreetNumber, gigBooking.ZipCode, gigBooking.City, gigBooking.ClientName, gigBooking.ClientEmail, gigBooking.ClientPhone, gigBooking.Venue);


        return gigBookingDto;

    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct)
    {
        var result = await _collection.DeleteOneAsync(
            Builders<GigBooking>.Filter.Eq("Id", id), ct);

        if (result.DeletedCount == 0)
            throw new NotFoundException("No gig found.");

        return true;
    }


    public async Task<GigBooking> FindByIdAsync(string id, CancellationToken ct)
    {

        if (string.IsNullOrWhiteSpace(id))
            throw new ValidationException("Id is required");

        FilterDefinition<GigBooking> filter = Builders<GigBooking>.Filter.Eq("Id", id);

        var gigBooking = await _collection.Find(filter).FirstOrDefaultAsync(ct) ?? throw new NotFoundException("No gig found.");

        return gigBooking;

    }

}