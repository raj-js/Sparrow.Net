using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Sparrow.Security.Data
{
    public class PersistedGrantDbContext : PersistedGrantDbContext<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext(DbContextOptions<PersistedGrantDbContext> options, OperationalStoreOptions storeOptions)
            : base(options, storeOptions)
        {

        }
    }
}
