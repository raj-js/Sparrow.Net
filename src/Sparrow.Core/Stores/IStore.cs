using System;
using System.Threading.Tasks;

namespace Sparrow.Core.Stores
{
    public interface IStore<TEntity, TKey> :
        ICreateStore<TEntity, TKey>,
        IRemoveStore<TEntity, TKey>,
        IUpdateStore<TEntity, TKey>,
        IQueryStore<TEntity, TKey>

        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        void Save();

        Task SaveAsync();
    }
}
