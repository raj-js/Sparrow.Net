using System;

namespace Sparrow.Core.Stores
{
    public interface IStore<TEntity, TKey> :
        ICreateStore<TEntity, TKey>,
        IRemoveStore<TEntity, TKey>,
        IUpdateStore<TEntity, TKey>,
        IQueryStore<TEntity, TKey>

        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {

    }
}
