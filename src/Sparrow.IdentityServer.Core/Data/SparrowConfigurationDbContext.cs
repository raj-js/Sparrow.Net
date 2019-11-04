using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Sparrow.IdentityServer.Core.Data
{
    public class SparrowConfigurationDbContext : ConfigurationDbContext<SparrowConfigurationDbContext>
    {
        public SparrowConfigurationDbContext(DbContextOptions<SparrowConfigurationDbContext> options, 
            ConfigurationStoreOptions storeOptions) : base(options, storeOptions)
        {
        }
    }
}
