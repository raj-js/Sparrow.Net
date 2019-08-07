using Castle.MicroKernel.Registration;
using Sparrow.Core.Dependency;
using Sparrow.EntityFrameworkCore.Configurations;

namespace Sparrow.EntityFrameworkCore
{
    public static class SparrowEfCoreExtensions
    {
        public static void AddEfCoreDependencies(this IIocManager iocManager)
        {
            iocManager.Register<IEfCoreConfiguration, EfCoreConfiguration>();

            iocManager.RegisterAssemblyByConvention(typeof(SparrowEfCoreExtensions).Assembly);

            iocManager.IocContainer.Register(
                Component.For(typeof(IDbContextProvider<>))
                    .ImplementedBy(typeof(DbContextProvider<>))
                    .LifestyleTransient()
                );
        }
    }
}
