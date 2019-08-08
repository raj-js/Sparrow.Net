using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Domain.Uow;

namespace Sparrow.EntityFrameworkCore.Uow
{
    public interface IEfCoreTransactionStrategy
    {
        void InitOptions(UowOptions options);

        DbContext CreateDbContext<TDbContext>(string connectionString) where TDbContext : DbContext;

        void Commit();

        void Dispose();
    }
}
