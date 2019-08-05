using Castle.MicroKernel.Registration;
using Sparrow.Core.Dependency;
using Sparrow.EntityFrameworkCore.Uow;

namespace Sparrow.EntityFrameworkCore
{
    public static class SparrowEfCoreExtensions
    {
        public static void AddEfCoreDependencies(this IIocManager iocManager)
        {
            iocManager.RegisterAssemblyByConvention(typeof(EfCoreUow).Assembly);

            iocManager.IocContainer.Register(
                Component.For(typeof(IDbContextProvider<>))
                    .ImplementedBy(typeof(DbContextProvider<>))
                    .LifestyleTransient()
                );
        }
    }
}
