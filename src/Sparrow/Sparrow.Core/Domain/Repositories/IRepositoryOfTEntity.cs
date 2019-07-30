using Sparrow.Core.Domain.Entities;

namespace Sparrow.Core.Domain.Repositories
{
    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型， 唯一标识类型为 int</typeparam>
    public interface IRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : class, IEntity
    {

    }
}
