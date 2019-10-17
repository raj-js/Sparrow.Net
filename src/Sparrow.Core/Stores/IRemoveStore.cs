using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sparrow.Core.Stores
{
    public interface IRemoveStore<TEntity, TKey> 
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        bool Remove(TEntity entity);

        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(TEntity entity);

        /// <summary>
        /// 删除指定唯一标识的实体
        /// </summary>
        /// <param name="id"></param>
        bool Remove(TKey id);

        /// <summary>
        /// 异步删除指定唯一标识的实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(TKey id);

        /// <summary>
        /// 删除满足 predicate 的实体
        /// </summary>
        /// <param name="predicate"></param>
        bool Remove(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步删除满足 predicate 的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
