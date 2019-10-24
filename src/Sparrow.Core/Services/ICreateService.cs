using Sparrow.Core.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparrow.Core.Services
{
    public interface ICreateService<TEntity, TKey, TCreateDTO, TDTO>
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="createDTO"></param>
        /// <returns></returns>
        OpResponse<TDTO> Create(TCreateDTO createDTO);

        /// <summary>
        /// 异步新增实体
        /// </summary>
        /// <param name="createDTO"></param>
        /// <returns></returns>
        Task<OpResponse<TDTO>> CreateAsync(TCreateDTO createDTO);

        /// <summary>
        /// 新增实体并返回唯一标识
        /// </summary>
        /// <param name="createDTO"></param>
        /// <returns></returns>
        OpResponse<TKey> CreateAndGetId(TCreateDTO createDTO);

        /// <summary>
        /// 异步新增实体并返回唯一标识
        /// </summary>
        /// <param name="createDTO"></param>
        /// <returns></returns>
        Task<OpResponse<TKey>> CreateAndGetIdAsync(TCreateDTO createDTO);

        /// <summary>
        /// 新增多个实体
        /// </summary>
        /// <param name="createDTOs"></param>
        /// <returns></returns>
        OpResponse CreateMany(IEnumerable<TCreateDTO> createDTOs);

        /// <summary>
        /// 新增多个实体
        /// </summary>
        /// <param name="createDTOs"></param>
        /// <returns></returns>
        Task<OpResponse> CreateManyAsync(IEnumerable<TCreateDTO> createDTOs);
    }
}
