using Sparrow.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Sparrow.EntityFrameworkCore
{
    public interface IDbContextEntityFinder
    {
        IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType);
    }
}
