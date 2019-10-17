using AutoMapper;
using Sparrow.Core.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparrow.Core.Services
{
    public abstract class ServiceBase
        <
        TEntity, TKey,
        TCreateDTO, TUpdateDTO,
        TDTO
        > :

        ICreateService<TEntity, TKey, TCreateDTO, TDTO>,
        IRemoveService<TEntity, TKey>,
        IUpdateService<TEntity, TKey, TUpdateDTO, TDTO>,
        IQueryService<TEntity, TKey, TDTO>,
        ICURLService
        <
        TEntity, TKey,
        TCreateDTO, TUpdateDTO,
        TDTO
        >

        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        protected IMapper Mapper { get; private set; }
        private readonly ICURLStore<TEntity, TKey> _store;

        public ServiceBase(IMapper mapper, ICURLStore<TEntity, TKey> store)
        {
            Mapper = mapper;
            _store = store;
        }

        #region protected

        protected virtual TEntity ToEntity(object dto) => Mapper.Map<TEntity>(dto);
        protected virtual TDTO ToDTO(TEntity entity) => Mapper.Map<TDTO>(entity);

        #endregion

        public long Count()
        {
            return _store.Count();
        }

        public Task<long> CountAsync()
        {
            return _store.CountAsync();
        }

        public TDTO Create(TCreateDTO createDTO)
        {
            var entity = _store.Create(ToEntity(createDTO));
            return ToDTO(entity);
        }

        public TKey CreateAndGetId(TCreateDTO createDTO)
        {
            return _store.CreateAndGetId(ToEntity(createDTO));
        }

        public Task<TKey> CreateAndGetIdAsync(TCreateDTO createDTO)
        {
            return _store.CreateAndGetIdAsync(ToEntity(createDTO));
        }

        public async Task<TDTO> CreateAsync(TCreateDTO createDTO)
        {
            var entity = await _store.CreateAsync(ToEntity(createDTO));
            return ToDTO(entity);
        }

        public void CreateMany(IEnumerable<TCreateDTO> createDTOs)
        {
            var entities = createDTOs.Select(s => ToEntity(s));
            _store.CreateMany(entities);
        }

        public Task CreateManyAsync(IEnumerable<TCreateDTO> createDTOs)
        {
            var entities = createDTOs.Select(s => ToEntity(s));
            return _store.CreateManyAsync(entities);
        }

        public TDTO Find(TKey id)
        {
            var entity = _store.Find(id);
            return ToDTO(entity);
        }

        public async Task<TDTO> FindAsync(TKey id)
        {
            var entity = await _store.FindAsync(id);
            return ToDTO(entity);
        }

        public bool Remove(TKey id)
        {
            return _store.Remove(id);
        }

        public Task<bool> RemoveAsync(TKey id)
        {
            return _store.RemoveAsync(id);
        }

        public TDTO Update(TUpdateDTO updateDTO)
        {
            var entity = ToEntity(updateDTO);
            return ToDTO(_store.Update(entity));
        }

        public TDTO Update(TKey id, params (string Field, object Value)[] selectors)
        {
            return ToDTO(_store.Update(id, selectors));
        }

        public async Task<TDTO> UpdateAsync(TUpdateDTO updateDTO)
        {
            var entity = await _store.UpdateAsync(ToEntity(updateDTO));
            return ToDTO(entity);
        }

        public async Task<TDTO> UpdateAsync(TKey id, params (string Field, object Value)[] selectors)
        {
            var entity = await _store.UpdateAsync(id, selectors);
            return ToDTO(entity);
        }

        public async Task<(List<TDTO> List, long Total)> PageQuery(int pageIndex, int pageSize, params (string Field, bool IsAsc)[] sortFields)
        {
            var paged = await _store.PageQuery(s => true, sortFields, pageIndex, pageSize);

            return (
                paged.Entities.Select(s => ToDTO(s)).ToList(),
                paged.Total
                );
        }
    }
}
