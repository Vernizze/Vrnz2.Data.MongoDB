using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vrnz2.Data.MongoDB.Connections;
using Vrnz2.Data.MongoDB.Interfaces.Repositories;
using Vrnz2.Data.MongoDB.Repositories;
using Vrnz2.Data.MongoDB.VOs;

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
            this._connection = Connection.GetInstance(database, collection, connection_string);
        }

        #endregion

        #region Methods 

        public void Dispose()
            => this._connection.StopClient();

        public async Task Add(string jsonValue)
        {
            var document = BsonSerializer.Deserialize<BsonDocument>(jsonValue);

            await Add(document);
        }

        public async Task Add<TEntity>(TEntity entity)
        {
            using (var rep = new Persistence(this._connection))
                await rep.Add(entity);
        }

        public async Task<TEntity> GetById<TEntity>(string id)
            where TEntity
                : IMongoDbEntity
        {
            using (var rep = new Persistence(this._connection))
                return await rep.GetById<TEntity>(id);
        }

        public async Task<IList<TEntity>> GetMany<TEntity>(Expression<Func<TEntity, bool>> expression)
        {
            using (var rep = new Persistence(this._connection))
                return await rep.GetMany(expression);
        }
        public async Task<long> GetCount<TEntity>(Expression<Func<TEntity, bool>> expression)
        {
            using (var rep = new Persistence(this._connection))
                return await rep.GetCount(expression);
        }
        public async Task<long> GetCount<TEntity>(string filter)
        {
            using (var rep = new Persistence(this._connection))
                return await rep.GetCount<TEntity>(filter);
        }

        public async Task<IList<TEntity>> GetManyWithPage<TEntity>(Expression<Func<TEntity, bool>> expression, int page = 0, int pageSize = 0, string sortDefinition = null, bool ascending = true)
        {
            using (var rep = new Persistence(this._connection))
                return await rep.GetManyWithPage(expression, page, pageSize, sortDefinition, ascending);
        }

        public async Task<IList<TEntity>> GetManyWithPage<TEntity>(ParamValue param)
        {
            using (var rep = new Persistence(this._connection))
                return await rep.GetManyWithPage<TEntity>(param);
        }

        public async Task<IList<TEntity>> GetMany<TEntity>(Expression<Func<TEntity, bool>> expression, SortDefinition<TEntity> sortDefinition = null, int? limit = null)
        {
            using (var rep = new Persistence(this._connection))
                return await rep.GetMany(expression, sortDefinition, limit);
        }

        public async Task<IList<TEntity>> GetMany<TEntity>(string filter)
        {
            using (var rep = new Persistence(this._connection))
                return await rep.GetMany<TEntity>(filter);
        }

        public async Task Remove<TEntity>(TEntity entity)
            where TEntity
                : IMongoDbEntity
        {
            using (var rep = new Persistence(this._connection))
                await rep.Remove(entity);
        }

        public async Task Update<TEntity>(TEntity entity)
            where TEntity
                : IMongoDbEntity
        {
            using (var rep = new Persistence(this._connection))
                await rep.Update(entity);
        }

        public async Task Update<TEntity, TField>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TField>> field, TField value)
            where TEntity
                : IMongoDbEntity
        {
            using (var rep = new Persistence(this._connection))
                await rep.Update(exp, field, value);
        }

        public Task UpdateManyAsync<TEntity>(Expression<Func<TEntity, bool>> exp, UpdateDefinition<TEntity> update)
            where TEntity : IMongoDbEntity
        {
            using (var rep = new Persistence(this._connection))
                return rep.UpdateManyAsync(exp, update);
        }

        #endregion
    }
}
