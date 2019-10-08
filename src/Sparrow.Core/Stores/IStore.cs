using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sparrow.Core
{
    /// <summary>
    /// IStore 接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPKey">实体唯一标识类型</typeparam>
    public interface IStore<TEntity, TPKey>
        where TEntity : class, IEntity<TPKey>
        where TPKey : IEquatable<TPKey>
    {
        #region 查询

        /// <summary>
        /// 获取所有实体作为 IQueryable<>
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 根据唯一标识获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(TPKey id);

        /// <summary>
        /// 异步根据唯一标识获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(TPKey id);

        /// <summary>
        /// 根据 prediacate 查询唯一实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步根据 predicate 查询唯一实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据 predicate 查询第一个满足条件的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步根据 predicate 查询第一个满足条件的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region 新增

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// 异步新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// 新增实体并返回唯一标识
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TPKey InsertAndGetId(TEntity entity);

        /// <summary>
        /// 异步新增实体并返回唯一标识
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TPKey> InsertAndGetIdAsync(TEntity entity);

        /// <summary>
        /// 新增或者更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity InsertOrUpdate(TEntity entity);

        /// <summary>
        /// 异步新增或者更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> InsertOrUpdateAsync(TEntity entity);

        #endregion

        #region 修改

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
        TEntity Update(TPKey id, Action<TEntity> updateAction);

        /// <summary>
        /// 异步根据唯一标识更新实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateAction"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TPKey id, Func<TEntity, Task> updateAction);

        #endregion

        #region 删除

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// 删除指定唯一标识的实体
        /// </summary>
        /// <param name="id"></param>
        void Delete(TPKey id);

        /// <summary>
        /// 异步删除指定唯一标识的实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(TPKey id);

        /// <summary>
        /// 删除满足 predicate 的实体
        /// </summary>
        /// <param name="predicate"></param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步删除满足 predicate 的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region 聚合

        /// <summary>
        /// 获得实体总数
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// 异步获得实体总数
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();

        /// <summary>
        /// 获取满足 predicate 的实体总数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步获取满足 predicate 的实体总数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 获取实体总数
        /// </summary>
        /// <returns></returns>
        long LongCount();

        /// <summary>
        /// 异步获取实体总数
        /// </summary>
        /// <returns></returns>
        Task<long> LongCountAsync();

        /// <summary>
        /// 获取满足 predicate 的实体总数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        long LongCount(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步获取满足 predicate 的实体总数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion
    }
}
