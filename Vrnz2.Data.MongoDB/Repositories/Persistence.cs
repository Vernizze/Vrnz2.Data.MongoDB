using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vrnz2.Data.MongoDB.Interfaces.Connections;
using Vrnz2.Data.MongoDB.Interfaces.Repositories;

namespace Vrnz2.Data.MongoDB.Repositories
{
    internal class Persistence
        : IPersistence
    {
        #region Variables

        private IConnection _conn;
        private IMongoDatabase _database;
        private string _collectionName = string.Empty;

        #endregion

        #region Constructors

        public Persistence(Guid connectionId, IConnection conn)
        {
            this._conn = conn;

            var mongoClient = this._conn.GetClient(connectionId);

            this._database = mongoClient.MongoClient.GetDatabase(mongoClient.Database);
            this._collectionName = mongoClient.Collection;
        }

        #endregion

        #region Methods

        public void Dispose()
            => this._database = null;

        public async Task Add<TEntity>(TEntity entity)
            => await this._database
                    .GetCollection<TEntity>(this._collectionName)
                        .InsertOneAsync(entity);

        public async Task<TEntity> GetById<TEntity>(string id)
            where TEntity
                : IMongoDbEntity
            => await this._database
                .GetCollection<TEntity>(this._collectionName)
                    .FindAsync(i => i.Id == new ObjectId(id))
                        .ContinueWith(c => c.Result.FirstOrDefault());

        public async Task<IList<TEntity>> GetMany<TEntity>(Expression<Func<TEntity, bool>> filter, SortDefinition<TEntity> sortDefinition = null, int? limit = null)
        {
            var filterDefinition = Builders<TEntity>.Filter.Where(filter);
            var findOption = (sortDefinition != null || limit != null) ?
                                new FindOptions<TEntity, TEntity>() { Sort = sortDefinition, Limit = limit } :
                                null;

            var result = await this._database
                .GetCollection<TEntity>(this._collectionName)
                .FindAsync(filterDefinition, findOption);

            return result.ToList();
        }

        public async Task<IList<TEntity>> GetMany<TEntity>(string filter)
        {
            var result = await this._database
                .GetCollection<TEntity>(this._collectionName)
                .FindAsync(filter);

            return result.ToList();
        }

        public async Task Remove<TEntity>(TEntity entity)
            where TEntity
                : IMongoDbEntity
            => await this._database.GetCollection<TEntity>(this._collectionName)
                    .DeleteOneAsync(e => e.Id == entity.Id);

        public async Task Update<TEntity>(TEntity entity)
            where TEntity
                : IMongoDbEntity
            => await this._database.GetCollection<TEntity>(this._collectionName).ReplaceOneAsync(e => e.Id == entity.Id, entity);

        public async Task Update<TEntity, TField>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TField>> field, TField value)
            where TEntity
                : IMongoDbEntity
        {
            var filter = Builders<TEntity>.Filter.Where(exp);
            var update = Builders<TEntity>.Update.Set(field, value);

            await this._database.GetCollection<TEntity>(this._collectionName).UpdateManyAsync(filter, update);
        }

        #endregion
    }

    public class Updates<TEntity>
    {
        public Expression<Func<TEntity, bool>> Expression { get; set; }
    }
}
