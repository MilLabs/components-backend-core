﻿using MongoDB.Driver;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.MongoDB.Context;

namespace Optsol.Components.Infra.MongoDB.Repository
{
    public interface IMongoRepository<TEntity, TKey> :
        IReadRepository<TEntity, TKey>,
        IWriteRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {
        MongoContext Context { get; }

        IMongoCollection<TEntity> Set { get; }
    }
}