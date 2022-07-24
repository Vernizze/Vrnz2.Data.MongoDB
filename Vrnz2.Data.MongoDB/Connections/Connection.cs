using System;
using MongoDB.Driver;
using Vrnz2.Data.MongoDB.Interfaces.Connections;

namespace Vrnz2.Data.MongoDB.Connections
{
    internal class Connection
            : IConnection, IDisposable
    {
        #region Variables

        private static Connection _instance;

        private MongoClientHelper _mongoClientHelper;

        private object _lock = new object();

        #endregion

        private Connection() { }

        private void CreateClient(string database, string collection, string connection_string)
        {
            lock (this._lock)
            {
                _mongoClientHelper = new MongoClientHelper
                {
                    Database = database,
                    Collection = collection,
                    MongoClient = new MongoClient(connection_string)
                };
            }
        }

        public static Connection GetInstance(string database, string collection, string connection_string)
        {
            _instance = _instance ?? new Connection();

            _instance.CreateClient(database, collection, connection_string);

            return _instance;
        }

        public void Dispose()
        {
        }

        public MongoClientHelper GetClient()
            => _mongoClientHelper;

        public void StopClient()
        {
            var client = GetClient();

            client = null;
        }
    }
}
