using Blog.Core.Sparrow.DTOs.Response;
using Sparrow.Core;
using System;
using System.Threading.Tasks;

namespace Sparrow.Core.Services
{
    public interface IRemoveService<TEntity, TKey>
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 删除指定唯一标识的实体
        /// </summary>
        /// <param name="id"></param>
        OpResponse Remove(TKey id);

        /// <summary>
        /// 异步删除指定唯一标识的实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OpResponse> RemoveAsync(TKey id);
    }
}
