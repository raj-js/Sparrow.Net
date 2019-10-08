using Sparrow.Core;
using System.Threading.Tasks;

namespace Sparrow.EntityFrameworkCore.Stores
{
    public interface IEfStore<TEntity, TPKey> : IStore<TEntity, TPKey> where TEntity : class, IEntity<TPKey>
    {
        Task<int> SaveChanges();
    }
}
