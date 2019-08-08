using Sparrow.Core.Dependency;
using Sparrow.Core.Domain.Repositories;
using System;

namespace Sparrow.EntityFrameworkCore.Repositories
{
    public interface IEfGenericRepositoryRegistrar
    {
        void RegisterForDbContext(Type dbContextType, IIocManager iocManager, AutoRepositoryTypesAttribute defaultAutoRepositoryTypesAttribute);
    }
}
