using MongoDB.Driver;

namespace Vrnz2.Data.MongoDB.Connections
{
    public class MongoClientHelper
    {
        public string Database { get; set; }
        public string Collection { get; set; }
        public MongoClient MongoClient { get; set; }
    }
}
