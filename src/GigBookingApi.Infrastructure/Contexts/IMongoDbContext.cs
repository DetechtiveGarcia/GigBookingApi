using MongoDB.Driver;

namespace GigBookingApi.Infrastructure.Contexts;
public interface IMongoDbContext
{
    IMongoCollection<T> GetCollection<T>(string name);
}