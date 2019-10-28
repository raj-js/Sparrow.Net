using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparrow.Core.Stores
{
    public interface ICreateStore<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Create(TEntity entity);

        /// <summary>
        /// 异步新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> CreateAsync(TEntity entity);

        /// <summary>
        /// 新增实体并返回唯一标识
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TKey CreateAndGetId(TEntity entity);

        /// <summary>
        /// 异步新增实体并返回唯一标识
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TKey> CreateAndGetIdAsync(TEntity entity);

        /// <summary>
        /// 新增多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void CreateMany(IEnumerable<TEntity> entities);

        /// <summary>
        /// 新增多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task CreateManyAsync(IEnumerable<TEntity> entities);
    }
}
