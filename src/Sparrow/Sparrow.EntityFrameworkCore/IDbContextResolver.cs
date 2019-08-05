using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Sparrow.EntityFrameworkCore
{
    public interface IDbContextResolver
    {
        TDbContext Resolve<TDbContext>(string connectionString, IDbConnection existingConnection)
            where TDbContext : DbContext;
    }
}
