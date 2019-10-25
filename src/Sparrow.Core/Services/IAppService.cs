using System;

namespace Sparrow.Core.Services
{
    public interface IAppService 
    {
    
    }

    public interface IAppService
        <
        TEntity, TKey,
        TCreateDTO, TUpdateDTO,
        TDTO
        > :
        IAppService
        <
        TEntity, TKey,
        TCreateDTO, TUpdateDTO,
        TDTO, TDTO
        >,
        ICreateService<TEntity, TKey, TCreateDTO, TDTO>,
        IRemoveService<TEntity, TKey>,
        IUpdateService<TEntity, TKey, TUpdateDTO, TDTO>,
        IQueryService<TEntity, TKey, TDTO, TDTO>

        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {

    }

    public interface IAppService
        <
        TEntity, TKey,
        TCreateDTO, TUpdateDTO,
        TListItemDTO, TDTO
        > :
        ICreateService<TEntity, TKey, TCreateDTO, TDTO>,
        IRemoveService<TEntity, TKey>,
        IUpdateService<TEntity, TKey, TUpdateDTO, TDTO>,
        IQueryService<TEntity, TKey, TListItemDTO, TDTO>

        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {

    }
}
