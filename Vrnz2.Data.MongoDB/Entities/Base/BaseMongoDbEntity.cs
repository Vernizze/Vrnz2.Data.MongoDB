using MongoDB.Bson;
using System;
using Vrnz2.Data.MongoDB.Interfaces.Repositories;

namespace Vrnz2.Data.MongoDB.Entities.Base
{
    public class BaseMongoDbEntity
        : IMongoDbEntity
    {
        #region Attributes

        public ObjectId Id { get; set; }
        public DateTime ReceitpDatetTime { get; set; } = DateTime.UtcNow;
        public bool Processed { get; set; } = false;
        public DateTime? ProcessingDateTime { get; set; }

        #endregion        
    }
}
