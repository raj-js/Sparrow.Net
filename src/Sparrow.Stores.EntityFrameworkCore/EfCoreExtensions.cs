using Autofac;
using Sparrow.Core.Stores;

namespace Sparrow.Stores.EntityFrameworkCore
{
    public static class EfCoreExtensions
    {
        public static void AddSparrowEfCore(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EfCoreStore<,>)).As(typeof(ICreateStore<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfCoreStore<,>)).As(typeof(IRemoveStore<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfCoreStore<,>)).As(typeof(IUpdateStore<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfCoreStore<,>)).As(typeof(IQueryStore<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfCoreStore<,>)).As(typeof(IStore<,>)).InstancePerLifetimeScope();
        }
    }
}
