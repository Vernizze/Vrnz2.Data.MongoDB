﻿using System;
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

        private Connection() { }

        private void CreateClient(Guid connectionId, string database, string collection, string connection_string) 
        {
            lock (this._lock)
            {
                if (!_connections.TryGetValue(connectionId, out _))
                    _connections.Add(connectionId, new MongoClientHelper { Database = database, Collection = collection, MongoClient = new MongoClient(connection_string) });
            }
        }

        public static Connection GetInstance(Guid connectionId, string database, string collection, string connection_string)
        {
            _instance = _instance ?? new Connection();

            _instance.CreateClient(connectionId, database, collection, connection_string);

            return _instance;
        }

        public void Dispose()
        {
            foreach (var item in _connections)
                item.Value.MongoClient = null;

            _connections.Clear();
        }

        public MongoClientHelper GetClient(Guid connectionId)
        {
            if (_connections.TryGetValue(connectionId, out MongoClientHelper client))
                return client;
            else
                return default(MongoClientHelper);
        }

        public void StopClient(Guid connectionId)
        {
            var client = GetClient(connectionId);

            if (client != null) 
            {
                _connections.Remove(connectionId);
                client = null;
            }
        }
    }
}
