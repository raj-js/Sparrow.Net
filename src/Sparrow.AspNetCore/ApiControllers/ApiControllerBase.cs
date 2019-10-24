using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparrow.Core.DTOs.Paging;
using Sparrow.Core.DTOs.Responses;
using Sparrow.Core.Services;
using System;
using System.Threading.Tasks;
using static Sparrow.Core.DTOs.Responses.OpResponse;

namespace Sparrow.Core.ApiControllers
{
    [ApiController]
    public abstract class ApiControllerBase<TEntity, TKey, TCreateDTO, TUpdateDTO, TDTO> : ControllerBase
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        protected readonly IAppService<TEntity, TKey, TCreateDTO, TUpdateDTO, TDTO> CURLService;

        public ApiControllerBase(IAppService<TEntity, TKey, TCreateDTO, TUpdateDTO, TDTO> curlService)
        {
            CURLService = curlService;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [HttpGet("paging")]
        [Authorize]
        public virtual async Task<OpResponse<Paged<TDTO>>> Paging([FromQuery]PageQuery query)
        {
            var opResponse = await CURLService.PageQuery(query.PageIndex, query.PageSize, (query.Order, query.IsAsc));

            if (opResponse.IsSuccess)
                return Success(PagingHelper.From(opResponse.Data));

            return Failure<Paged<TDTO>>();
        }

        /// <summary>
        /// 根据唯一标识获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public virtual async Task<OpResponse<TDTO>> Get(TKey id)
        {
            return await CURLService.FindAsync(id);
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public virtual async Task<OpResponse<TDTO>> Post([FromBody]TCreateDTO dto)
        {
            return await CURLService.CreateAsync(dto);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public virtual async Task<OpResponse<TDTO>> Put([FromBody]TUpdateDTO dto)
        {
            return await CURLService.UpdateAsync(dto);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public virtual async Task<OpResponse> Delete(TKey id)
        {
            return await CURLService.RemoveAsync(id);
        }
    }
}
