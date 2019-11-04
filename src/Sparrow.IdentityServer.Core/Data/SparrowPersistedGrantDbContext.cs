using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Sparrow.IdentityServer.Core.Data
{
    public class SparrowPersistedGrantDbContext : PersistedGrantDbContext<SparrowPersistedGrantDbContext>
    {
        public SparrowPersistedGrantDbContext(DbContextOptions options, 
            OperationalStoreOptions storeOptions) : base(options, storeOptions)
        {
        }
    }
}
