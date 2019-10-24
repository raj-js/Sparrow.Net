using AutoMapper;
using Blog.Core.Sparrow.DTOs.Response;
using Sparrow.Core.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Blog.Core.Sparrow.DTOs.Response.OpResponse;

namespace Sparrow.Core.Services
{
    public class AppServiceBase
        <
        TEntity, TKey,
        TCreateDTO, TUpdateDTO,
        TDTO
        > :

        ICreateService<TEntity, TKey, TCreateDTO, TDTO>,
        IRemoveService<TEntity, TKey>,
        IUpdateService<TEntity, TKey, TUpdateDTO, TDTO>,
        IQueryService<TEntity, TKey, TDTO>,
        IAppService
        <
        TEntity, TKey,
        TCreateDTO, TUpdateDTO,
        TDTO
        >

        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        protected IMapper Mapper { get; private set; }
        protected ICURLStore<TEntity, TKey> Store { get; private set; }

        public AppServiceBase(IMapper mapper, ICURLStore<TEntity, TKey> store)
        {
            Mapper = mapper;
            Store = store;
        }

        #region protected

        protected virtual TEntity ToEntity(object dto) => Mapper.Map<TEntity>(dto);

        protected virtual TDTO ToDTO(TEntity entity) => Mapper.Map<TDTO>(entity);

        #endregion

        public OpResponse<long> Count()
        {
            return Success(Store.Count());
        }

        public async Task<OpResponse<long>> CountAsync()
        {
            return Success(await Store.CountAsync());
        }

        public OpResponse<TDTO> Create(TCreateDTO createDTO)
        {
            var entity = Store.Create(ToEntity(createDTO));
            return Success(ToDTO(entity));
        }

        public OpResponse<TKey> CreateAndGetId(TCreateDTO createDTO)
        {
            return Success(Store.CreateAndGetId(ToEntity(createDTO)));
        }

        public async Task<OpResponse<TKey>> CreateAndGetIdAsync(TCreateDTO createDTO)
        {
            return Success(await Store.CreateAndGetIdAsync(ToEntity(createDTO)));
        }

        public async Task<OpResponse<TDTO>> CreateAsync(TCreateDTO createDTO)
        {
            var entity = await Store.CreateAsync(ToEntity(createDTO));
            return Success(ToDTO(entity));
        }

        public OpResponse CreateMany(IEnumerable<TCreateDTO> createDTOs)
        {
            var entities = createDTOs.Select(s => ToEntity(s));
            Store.CreateMany(entities);
            return Success();
        }

        public async Task<OpResponse> CreateManyAsync(IEnumerable<TCreateDTO> createDTOs)
        {
            var entities = createDTOs.Select(s => ToEntity(s));
            await Store.CreateManyAsync(entities);
            return Success();
        }

        public OpResponse<TDTO> Find(TKey id)
        {
            var entity = Store.Find(id);
            return Success(ToDTO(entity));
        }

        public async Task<OpResponse<TDTO>> FindAsync(TKey id)
        {
            var entity = await Store.FindAsync(id);
            return Success(ToDTO(entity));
        }

        public OpResponse Remove(TKey id)
        {
            return Assert(Store.Remove(id));
        }

        public async Task<OpResponse> RemoveAsync(TKey id)
        {
            return Assert(await Store.RemoveAsync(id));
        }

        public OpResponse<TDTO> Update(TUpdateDTO updateDTO)
        {
            var entity = ToEntity(updateDTO);
            return Success(ToDTO(Store.Update(entity)));
        }

        public OpResponse<TDTO> Update(TKey id, params (string Field, object Value)[] selectors)
        {
            return Success(ToDTO(Store.Update(id, selectors)));
        }

        public async Task<OpResponse<TDTO>> UpdateAsync(TUpdateDTO updateDTO)
        {
            var entity = await Store.UpdateAsync(ToEntity(updateDTO));
            return Success(ToDTO(entity));
        }

        public async Task<OpResponse<TDTO>> UpdateAsync(TKey id, params (string Field, object Value)[] selectors)
        {
            var entity = await Store.UpdateAsync(id, selectors);
            return Success(ToDTO(entity));
        }

        public async Task<OpResponse<(List<TDTO> List, long Total)>> PageQuery(int pageIndex, int pageSize, params (string Field, bool IsAsc)[] sortFields)
        {
            var (Entities, Total) = await Store.PageQuery(s => true, sortFields, pageIndex, pageSize);

            return Success((
                Entities.Select(s => ToDTO(s)).ToList(),
                Total
                ));
        }
    }
}
