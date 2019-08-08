using Sparrow.Core.Domain.Repositories;
using Sparrow.EntityFrameworkCore.Repositories;

namespace Sparrow.EntityFrameworkCore
{
    public static class EfCoreAutoRepositoryTypes
    {
        public static AutoRepositoryTypesAttribute Default { get; private set; }

        static EfCoreAutoRepositoryTypes()
        {
            Default = new AutoRepositoryTypesAttribute(
                typeof(IRepository<>),
                typeof(IRepository<,>),
                typeof(EfCoreRepository<,>),
                typeof(EfCoreRepository<,,>)
            );
        }
    }
}
