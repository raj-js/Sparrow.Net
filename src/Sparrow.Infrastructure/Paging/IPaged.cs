using System.Collections.Generic;

namespace Sparrow.Infrastructure.Paging
{
    public interface IPaged<TEntity>
    {
        int Total { get; set; }

        IEnumerable<TEntity> Entities { get; set; }
    }
}
