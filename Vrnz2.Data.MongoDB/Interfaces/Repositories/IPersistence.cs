﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Vrnz2.Data.MongoDB.Interfaces.Repositories
{
    public interface IPersistence
        : IDisposable
    {
        Task Add<TEntity>(TEntity entity);
        Task<TEntity> GetById<TEntity>(string id) where TEntity : IMongoDbEntity;
        Task<IList<TEntity>> GetMany<TEntity>(Expression<Func<TEntity, bool>> expression, SortDefinition<TEntity> sortDefinition = null, int? limit = null);
        Task<IList<TEntity>> GetMany<TEntity>(string filter);
        Task Remove<TEntity>(TEntity entity) where TEntity : IMongoDbEntity;
        Task Update<TEntity>(TEntity entity) where TEntity : IMongoDbEntity;
    }
}
