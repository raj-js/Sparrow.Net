using Blog.Core.Sparrow.DTOs.Response;
using System;
using System.Threading.Tasks;

namespace Sparrow.Core.Services
{
    public interface IUpdateService<TEntity, TKey, TUpdateDTO, TDTO>
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="updateDTO"></param>
        /// <returns></returns>
        OpResponse<TDTO> Update(TUpdateDTO updateDTO);

        /// <summary>
        /// 异步更新实体
        /// </summary>
        /// <param name="updateDTO"></param>
        /// <returns></returns>
        Task<OpResponse<TDTO>> UpdateAsync(TUpdateDTO updateDTO);

        /// <summary>
        /// 部分更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selectors"></param>
        /// <returns></returns>
        OpResponse<TDTO> Update(TKey id, params (string Field, object Value)[] selectors);

        /// <summary>
        /// 异步部分更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selectors"></param>
        /// <returns></returns>
        Task<OpResponse<TDTO>> UpdateAsync(TKey id, params (string Field, object Value)[] selectors);
    }
}
