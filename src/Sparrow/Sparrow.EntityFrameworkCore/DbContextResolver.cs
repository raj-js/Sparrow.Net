using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Dependency;
using System;
using System.Data;
using Sparrow.EntityFrameworkCore.Configurations;

namespace Sparrow.EntityFrameworkCore
{
    public class DbContextResolver : IDbContextResolver, ITransientDependency
    {
        private readonly IIocResolver _iocResolver;

        public DbContextResolver(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public TDbContext Resolve<TDbContext>(string connectionString, IDbConnection existingConnection) 
            where TDbContext : DbContext
        {
            return _iocResolver.Resolve<TDbContext>(new { options = CreateOptions<TDbContext>(connectionString, existingConnection) });
        }

        private DbContextOptions<TDbContext> CreateOptions<TDbContext>(string connectionString, IDbConnection dbConnection) where TDbContext : DbContext
        {
            if (_iocResolver.IsRegistered<EfDbContextConfiguration<TDbContext>>())
            {
                var configuration = new EfDbContextConfiguration<TDbContext>(connectionString, dbConnection);

                using (var configurator = _iocResolver.ResolveAsDisposable<IEfDbContextConfigurator<TDbContext>>())
                {
                    configurator.Object.Configure(configuration);
                }

                return configuration.Builder.Options;
            }

            if (_iocResolver.IsRegistered<DbContextOptions<TDbContext>>())
            {
                return _iocResolver.Resolve<DbContextOptions<TDbContext>>();
            }

            throw new Exception($"Could not resolve DbContextOptions for {typeof(TDbContext).AssemblyQualifiedName}.");
        }
    }
}
