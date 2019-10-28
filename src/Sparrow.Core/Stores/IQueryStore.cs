using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sparrow.Core.Stores
{
    public interface IQueryStore<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 获取所有实体作为 IQueryable<>
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Query();

        /// <summary>
        /// 根据唯一标识获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Find(TKey id);

        /// <summary>
        /// 异步根据唯一标识获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(TKey id);

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

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sortFields">排序字段</param>
        /// <param name="pageIndex">页码 （从 1 开始）</param>
        /// <param name="pageSize">每页条目数</param>
        /// <returns>Entities 结果集， Total 总条目</returns>
        Task<(IEnumerable<TEntity> Entities, long Total)> PageQuery(Expression<Func<TEntity, bool>> predicate, (string Field, bool IsAsc)[] sortFields, int pageIndex, int pageSize);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sortFields">排序字段</param>
        /// <param name="pageIndex">页码 （从 1 开始）</param>
        /// <param name="pageSize">每页条目数</param>
        /// <returns>Entities 结果集， Total 总条目</returns>
        Task<(IEnumerable<TEntity> Entities, long Total)> PageQuery(Expression<Func<TEntity, bool>> predicate, (Expression<Func<TEntity, object>> Selector, bool IsAsc)[] sortFields, int pageIndex, int pageSize);

        /// <summary>
        /// 获得实体总数
        /// </summary>
        /// <returns></returns>
        long Count();

        /// <summary>
        /// 异步获得实体总数
        /// </summary>
        /// <returns></returns>
        Task<long> CountAsync();

        /// <summary>
        /// 获取满足 predicate 的实体总数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        long Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步获取满足 predicate 的实体总数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
