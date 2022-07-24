using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vrnz2.Data.MongoDB.VOs;

namespace Vrnz2.Data.MongoDB.Interfaces.Repositories
{
    public interface IPersistence
        : IDisposable
    {
        Task Add<TEntity>(TEntity entity);
        Task<TEntity> GetById<TEntity>(string id) where TEntity : IMongoDbEntity;
        Task<IList<TEntity>> GetMany<TEntity>(Expression<Func<TEntity, bool>> expression, SortDefinition<TEntity> sortDefinition = null, int? limit = null);
        Task<IList<TEntity>> GetMany<TEntity>(string filter);
        Task<IList<TEntity>> GetManyWithPage<TEntity>(ParamValue param);
        Task<IList<TEntity>> GetManyWithPage<TEntity>(Expression<Func<TEntity, bool>> filter, int page, int pageSize, string sortField = null, bool ascending = true);
        Task Remove<TEntity>(TEntity entity) where TEntity : IMongoDbEntity;
        Task Update<TEntity>(TEntity entity) where TEntity : IMongoDbEntity;
        Task Update<TEntity, TField>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TField>> field, TField value) where TEntity : IMongoDbEntity;
        Task UpdateManyAsync<TEntity>(Expression<Func<TEntity, bool>> exp, UpdateDefinition<TEntity> update) where TEntity : IMongoDbEntity;
        Task<long> GetCount<TEntity>(Expression<Func<TEntity, bool>> filter);
        Task<long> GetCount<TEntity>(string filter);
    }
}
