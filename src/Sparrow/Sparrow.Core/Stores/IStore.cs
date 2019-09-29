using System.Linq;
using System.Threading.Tasks;

namespace Sparrow.Core
{
    public interface IStore<TEntity, TPKey> where TEntity: class, IEntity<TPKey>
    {
        Task<IQueryable<TEntity>> Query();

        Task<TEntity> Single(TPKey id);

        Task<TEntity> Create(TEntity entity);

        Task<int> Delete(TEntity entity);

        Task<int> Delete(TPKey id);

        Task<int> Modify(TEntity entity);
    }
}
