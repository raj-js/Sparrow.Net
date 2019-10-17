using System;
using System.Threading.Tasks;

namespace Sparrow.Core.Stores
{
    public interface IUpdateStore<TEntity, TKey>
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// 异步更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// 根据唯一标识更新实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateAction"></param>
        /// <returns></returns>
        TEntity Update(TKey id, Action<TEntity> updateAction);

        /// <summary>
        /// 异步根据唯一标识更新实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateAction"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TKey id, Func<TEntity, Task> updateAction);

        /// <summary>
        /// 部分更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selectors"></param>
        /// <returns></returns>
        TEntity Update(TKey id, params (string Field, object Value)[] selectors);

        /// <summary>
        /// 异步部分更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selectors"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TKey id, params (string Field, object Value)[] selectors);
    }
}
