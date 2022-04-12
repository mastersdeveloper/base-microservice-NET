using MongoDB.Driver;

namespace BASE.MICRONET.History.Repositories
{
    public interface IMongoBookDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
