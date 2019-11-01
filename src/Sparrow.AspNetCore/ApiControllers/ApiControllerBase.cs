using Microsoft.AspNetCore.Mvc;
using Sparrow.Core.DTOs.Paging;
using Sparrow.Core.DTOs.Responses;
using Sparrow.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Sparrow.Core.DTOs.Responses.OpResponse;

namespace Sparrow.Core.ApiControllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
    }

    [ApiController]
    public abstract class ApiControllerBase<TEntity, TKey, TCreateDTO, TUpdateDTO, TDTO> :
        ApiControllerBase<TEntity, TKey, TCreateDTO, TUpdateDTO, TDTO, TDTO>
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public ApiControllerBase(IAppService<TEntity, TKey, TCreateDTO, TUpdateDTO, TDTO, TDTO> appService) : base(appService)
        {
        }
    }

    [ApiController]
    public abstract class ApiControllerBase<TEntity, TKey, TCreateDTO, TUpdateDTO, TListItemDTO, TDTO> : ApiControllerBase
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        protected readonly IAppService<TEntity, TKey, TCreateDTO, TUpdateDTO, TListItemDTO, TDTO> AppService;

        public ApiControllerBase(IAppService<TEntity, TKey, TCreateDTO, TUpdateDTO, TListItemDTO, TDTO> appService)
        {
            AppService = appService;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public virtual async Task<OpResponse<List<TListItemDTO>>> All() 
        {
            return await 
                Task<OpResponse<List<TListItemDTO>>>
                .Factory
                .StartNew(AppService.All);   
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [HttpGet("paging")]
        // [Authorize]
        public virtual async Task<OpResponse<Paged<TListItemDTO>>> Paging([FromQuery]PageQuery query)
        {
            var opResponse = await AppService.PageQuery(query.PageIndex, query.PageSize, (query.Order, query.IsAsc));

            if (opResponse.IsSuccess)
                return Success(PagingHelper.From(opResponse.Data));

            return Failure<Paged<TListItemDTO>>();
        }

        /// <summary>
        /// 根据唯一标识获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        // [Authorize]
        public virtual async Task<OpResponse<TDTO>> Get([FromRoute]TKey id)
        {
            return await AppService.FindAsync(id);
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        // [Authorize]
        public virtual async Task<OpResponse<TDTO>> Post([FromBody]TCreateDTO dto)
        {
            return await AppService.CreateAsync(dto);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        // [Authorize]
        public virtual async Task<OpResponse<TDTO>> Put([FromBody]TUpdateDTO dto)
        {
            return await AppService.UpdateAsync(dto);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        // [Authorize]
        public virtual async Task<OpResponse> Delete([FromRoute]TKey id)
        {
            return await AppService.RemoveAsync(id);
        }
    }
}
