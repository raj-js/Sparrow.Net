using Castle.MicroKernel.Registration;
using Microsoft.EntityFrameworkCore;
using Sparrow.Core.Dependency;
using Sparrow.EntityFrameworkCore.Configurations;
using Sparrow.EntityFrameworkCore.Repositories;
using System.Linq;
using System.Reflection;

namespace Sparrow.EntityFrameworkCore
{
    public static class EfCoreExtensions
    {
        public static void AddEfCoreDependencies(this IIocManager iocManager, Assembly assembly)
        {
            iocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            iocManager.Register<IEfCoreConfiguration, EfCoreConfiguration>();

            iocManager.IocContainer.Register(
                Component.For(typeof(IDbContextProvider<>))
                    .ImplementedBy(typeof(DbContextProvider<>))
                    .LifestyleTransient()
                );

            RegisterGenericRepositoriesAndMatchDbContexts(iocManager, assembly);
        }

        private static void RegisterGenericRepositoriesAndMatchDbContexts(IIocManager iocManager, Assembly assembly)
        {
            var dbContextTypes = assembly.GetTypes()
                .Where(type =>
                    type.IsPublic &&
                    !type.IsAbstract &&
                    type.IsClass &&
                    typeof(DbContext).IsAssignableFrom(type));

            using (var scope = iocManager.CreateScope())
            {
                var repositoryRegistrar = scope.Resolve<IEfGenericRepositoryRegistrar>();

                foreach (var dbContextType in dbContextTypes)
                {
                    repositoryRegistrar.RegisterForDbContext(dbContextType, iocManager, EfCoreAutoRepositoryTypes.Default);
                }
            }
        }
    }
}
