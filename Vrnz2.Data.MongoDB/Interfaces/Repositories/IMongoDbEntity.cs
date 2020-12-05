using MongoDB.Bson;

namespace Vrnz2.Data.MongoDB.Interfaces.Repositories
{
    public interface IMongoDbEntity
    {
        ObjectId Id { get; set; }
    }
}
