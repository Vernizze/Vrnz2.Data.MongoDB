﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vrnz2.Data.MongoDB.Connections;
using Vrnz2.Data.MongoDB.Interfaces.Repositories;
using Vrnz2.Data.MongoDB.Repositories;

namespace Vrnz2.Data.MongoDB
{
    public class MongoDB
        : IDisposable
    {
        #region Variables 

        private Connection _connection;

        #endregion

        #region Constructors 

        public MongoDB(string connection_string, string collection, string database)
        {
            ConnectionId = Guid.NewGuid();

            this._connection = Connection.GetInstance(ConnectionId, database, collection, connection_string);
        }

        #endregion

        #region Attributes

        public Guid ConnectionId { get; }

        #endregion

        #region Methods 

        public void Dispose()
            => this._connection.StopClient(ConnectionId);


        public async Task Add<TEntity>(TEntity entity)
        {
            using (var rep = new Persistence(ConnectionId, this._connection))
                await rep.Add(entity);
        }

        public async Task<TEntity> GetById<TEntity>(string id)
            where TEntity
                : IMongoDbEntity
        {
            using (var rep = new Persistence(ConnectionId, this._connection))
                return await rep.GetById<TEntity>(id);
        }

        public async Task<IList<TEntity>> GetMany<TEntity>(Expression<Func<TEntity, bool>> expression)
        {
            using (var rep = new Persistence(ConnectionId, this._connection))
                return await rep.GetMany(expression);
        }

        public async Task<IList<TEntity>> GetMany<TEntity>(Expression<Func<TEntity, bool>> expression, SortDefinition<TEntity> sortDefinition = null, int? limit = null)
        {
            using (var rep = new Persistence(ConnectionId, this._connection))
                return await rep.GetMany(expression, sortDefinition, limit);
        }

        public async Task<IList<TEntity>> GetMany<TEntity>(string filter)
        {
            using (var rep = new Persistence(ConnectionId, this._connection))
                return await rep.GetMany<TEntity>(filter);
        }

        public async Task Remove<TEntity>(TEntity entity)
            where TEntity
                : IMongoDbEntity
        {
            using (var rep = new Persistence(ConnectionId, this._connection))
                await rep.Remove(entity);
        }

        public async Task Update<TEntity>(TEntity entity)
            where TEntity
                : IMongoDbEntity
        {
            using (var rep = new Persistence(ConnectionId, this._connection))
                await rep.Update(entity);
        }

        public async Task Update<TEntity, TField>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TField>> field, TField value)
            where TEntity
                : IMongoDbEntity
        {
            using (var rep = new Persistence(ConnectionId, this._connection))
                await rep.Update(exp, field, value);
        }

        #endregion
    }
}
