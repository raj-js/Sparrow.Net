using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Dependency;
using Sparrow.Core.Domain.Uow;

namespace Sparrow.EntityFrameworkCore.Uow
{
    public interface IEfCoreTransactionStrategy
    {
        void InitArgs(UowArgs args);

        DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver)
            where TDbContext : DbContext;

        void Commit();

        void Dispose(IIocResolver iocResolver);
    }
}
