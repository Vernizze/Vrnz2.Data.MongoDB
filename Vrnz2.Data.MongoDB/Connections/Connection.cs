using System;
using MongoDB.Driver;
using Vrnz2.Data.MongoDB.Interfaces.Connections;
using System.Collections.Generic;

namespace Vrnz2.Data.MongoDB.Connections
{
    internal class Connection
        : IConnection, IDisposable
    {
        #region Variables

        private Dictionary<Guid, MongoClientHelper> _connections = new Dictionary<Guid, MongoClientHelper>();

        private static Connection _instance;
        private object _lock = new object();

        #endregion

        private Connection(Guid connectionId, string database, string collection, string connection_string)
        {
            lock (this._lock)
            {
                var client = new MongoClient(connection_string);

                _connections.Add(connectionId, new MongoClientHelper { Database = database, Collection = collection, MongoClient = client });
            }
        }

        public static Connection GetInstance(Guid connectionId, string database, string collection, string connection_string)
        {
            _instance = _instance ?? new Connection(connectionId, database, collection, connection_string);

            return _instance;
        }

        public void Dispose()
        {
            foreach (var item in _connections)
                item.Value.MongoClient = null;
        }

        public MongoClientHelper GetClient(Guid connectionId)
        {
            if (_connections.TryGetValue(connectionId, out MongoClientHelper client))
                return client;
            else
                return default(MongoClientHelper);
        }
    }
}
