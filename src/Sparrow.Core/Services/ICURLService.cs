using Sparrow.Core;
using System;

namespace Sparrow.Core.Services
{
    public interface ICURLService
        <
        TEntity, TKey,
        TCreateDTO, TUpdateDTO,
        TDTO
        > :
        ICreateService<TEntity, TKey, TCreateDTO, TDTO>,
        IRemoveService<TEntity, TKey>,
        IUpdateService<TEntity, TKey, TUpdateDTO, TDTO>,
        IQueryService<TEntity, TKey, TDTO>

        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {

    }
}
