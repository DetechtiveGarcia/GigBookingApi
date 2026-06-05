using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GigBookingApi.Infrastructure.Contexts;
public class MongoDbContext(IOptions<MongoDbSettings> settings) : IMongoDbContext
{
    private readonly IMongoDatabase _database = new MongoClient(settings.Value.ConnectionURI).GetDatabase(settings.Value.DatabaseName);

    public IMongoCollection<T> GetCollection<T>(string name) => _database.GetCollection<T>(name);
}