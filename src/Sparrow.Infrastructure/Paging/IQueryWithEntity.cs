using System;
using System.Reflection;

namespace Sparrow.Infrastructure.Paging
{
    public interface IQuery<TEntity> : IQuery
    {
        TEntity Entity { get; set; }

        Func<TEntity, PropertyInfo> SortByProperty { get; set; }
    }
}
