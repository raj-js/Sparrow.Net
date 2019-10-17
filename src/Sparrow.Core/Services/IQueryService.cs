using Sparrow.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparrow.Core.Services
{
    public interface IQueryService<TEntity, TKey, TDTO>
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sortFields">排序字段</param>
        /// <param name="pageIndex">页码 （从 1 开始）</param>
        /// <param name="pageSize">每页条目数</param>
        /// <returns>Entities 结果集， Total 总条目</returns>
        Task<(List<TDTO> List, long Total)> PageQuery(int pageIndex, int pageSize, params (string Field, bool IsAsc)[] sortFields);

        /// <summary>
        /// 根据唯一标识获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TDTO Find(TKey id);

        /// <summary>
        /// 异步根据唯一标识获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TDTO> FindAsync(TKey id);

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
    }
}
