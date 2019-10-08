using System.Linq;
using System.Threading.Tasks;

namespace Sparrow.Core
{
    public interface IStore<TEntity, TPKey> where TEntity: class, IEntity<TPKey>
    {
        Task<TEntity> Create(TEntity entity);

        Task<int> Delete(TEntity entity);

        Task<int> Modify(TEntity entity);

        Task<IQueryable<TEntity>> All();

        Task<TEntity> Find(TPKey id);
    }
}
